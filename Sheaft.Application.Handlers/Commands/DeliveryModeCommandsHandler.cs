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

namespace Sheaft.Application.Handlers
{
    public class DeliveryModeCommandsHandler : CommandsHandler,
        IRequestHandler<CreateDeliveryModeCommand, CommandResult<Guid>>,
        IRequestHandler<UpdateDeliveryModeCommand, CommandResult<bool>>,
        IRequestHandler<DeleteDeliveryModeCommand, CommandResult<bool>>
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
                foreach(var oh in request.OpeningHours)
                {
                    openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                }

                var entity = new DeliveryMode(Guid.NewGuid(), request.Kind, user.Company, request.LockOrderHoursBeforeDelivery, deliveryModeAddress, openingHours, request.Name, request.Description);
                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return OkResult(entity.Id);
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

                _context.DeliveryModes.Update(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeleteDeliveryModeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<DeliveryMode>(request.Id, token);
                _context.DeliveryModes.Remove(entity);

                return OkResult(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}