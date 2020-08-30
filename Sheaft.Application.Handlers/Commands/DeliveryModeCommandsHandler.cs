using Sheaft.Infrastructure.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Sheaft.Exceptions;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Handlers
{
    public class DeliveryModeCommandsHandler : CommandsHandler,
        IRequestHandler<CreateDeliveryModeCommand, CommandResult<Guid>>,
        IRequestHandler<UpdateDeliveryModeCommand, CommandResult<bool>>,
        IRequestHandler<DeleteDeliveryModeCommand, CommandResult<bool>>,
        IRequestHandler<RestoreDeliveryModeCommand, CommandResult<bool>>
    {
        private readonly IAppDbContext _context;

        public DeliveryModeCommandsHandler(
            IAppDbContext context, 
            ILogger<DeliveryModeCommandsHandler> logger) : base(logger)
        {
            _context = context;
        }

        public async Task<CommandResult<Guid>> Handle(CreateDeliveryModeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);

                SimpleAddress deliveryModeAddress = null;
                if (request.Address != null)
                {
                    deliveryModeAddress = new SimpleAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Longitude, request.Address.Latitude);
                }

                var openingHours = new List<TimeSlotHour>();
                if (request.OpeningHours != null)
                {
                    foreach (var oh in request.OpeningHours)
                    {
                        openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                    }
                }

                var entity = new DeliveryMode(Guid.NewGuid(), request.Kind, user.Company, request.LockOrderHoursBeforeDelivery, deliveryModeAddress, openingHours, request.Name, request.Description);
                
                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return Ok(entity.Id);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdateDeliveryModeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var entity = await _context.GetByIdAsync<DeliveryMode>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetDescription(request.Description);
                entity.SetLockOrderHoursBeforeDelivery(request.LockOrderHoursBeforeDelivery);
                entity.SetKind(request.Kind);

                if (request.Address != null)
                {
                    entity.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Longitude, request.Address.Latitude);
                }

                if (request.OpeningHours != null)
                {
                    var openingHours = new List<TimeSlotHour>();
                    foreach (var oh in request.OpeningHours)
                    {
                        openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                    }

                    entity.SetOpeningHours(openingHours);
                }

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteDeliveryModeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<DeliveryMode>(request.Id, token);
                var activeAgreements = await _context.Agreements.CountAsync(a => a.Delivery.Id == entity.Id && !a.RemovedOn.HasValue, token);
                if (activeAgreements > 0)
                    throw new BadRequestException(MessageKind.DeliveryMode_CannotRemove_With_Active_Agreements, entity.Name, activeAgreements);

                _context.Remove(entity);
                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(RestoreDeliveryModeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.DeliveryModes.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                entity.Restore();

                _context.Update(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}