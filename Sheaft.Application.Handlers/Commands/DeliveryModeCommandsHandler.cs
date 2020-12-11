using Sheaft.Application.Interop;
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
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Handlers
{
    public class DeliveryModeCommandsHandler : ResultsHandler,
        IRequestHandler<CreateDeliveryModeCommand, Result<Guid>>,
        IRequestHandler<UpdateDeliveryModeCommand, Result<bool>>,
        IRequestHandler<DeleteDeliveryModeCommand, Result<bool>>,
        IRequestHandler<RestoreDeliveryModeCommand, Result<bool>>,
        IRequestHandler<SetDeliveryModesAvailabilityCommand, Result<bool>>,
        IRequestHandler<SetDeliveryModeAvailabilityCommand, Result<bool>>
    {
        public DeliveryModeCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeliveryModeCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateDeliveryModeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.RequestUser.Id, token);

                DeliveryAddress deliveryModeAddress = null;
                if (request.Address != null)
                {
                    deliveryModeAddress = new DeliveryAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country, request.Address.Longitude, request.Address.Latitude);
                }

                var openingHours = new List<TimeSlotHour>();
                if (request.OpeningHours != null)
                {
                    foreach (var oh in request.OpeningHours)
                    {
                        openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                    }
                }

                var entity = new DeliveryMode(Guid.NewGuid(), request.Kind, producer, request.Available, request.LockOrderHoursBeforeDelivery, deliveryModeAddress, openingHours, request.Name, request.Description);
                entity.SetAutoAcceptRelatedPurchaseOrders(request.AutoAcceptRelatedPurchaseOrder);
                entity.SetAutoCompleteRelatedPurchaseOrders(request.AutoCompleteRelatedPurchaseOrder);

                if (request.Kind == DeliveryKind.Collective || request.Kind == DeliveryKind.Farm || request.Kind == DeliveryKind.Market)
                    producer.CanDirectSell = true;

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return Ok(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdateDeliveryModeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.RequestUser.Id, token);
                var entity = await _context.GetByIdAsync<DeliveryMode>(request.Id, token);

                entity.SetName(request.Name);
                entity.SetAvailability(request.Available);
                entity.SetDescription(request.Description);
                entity.SetLockOrderHoursBeforeDelivery(request.LockOrderHoursBeforeDelivery);
                entity.SetKind(request.Kind); 
                entity.SetAutoAcceptRelatedPurchaseOrders(request.AutoAcceptRelatedPurchaseOrder);
                entity.SetAutoCompleteRelatedPurchaseOrders(request.AutoCompleteRelatedPurchaseOrder);

                if (request.Address != null)
                    entity.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City, request.Address.Country, request.Address.Longitude, request.Address.Latitude);
                
                if (request.OpeningHours != null)
                {
                    var openingHours = new List<TimeSlotHour>();
                    foreach (var oh in request.OpeningHours)
                    {
                        openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                    }

                    entity.SetOpeningHours(openingHours);
                }

                if (request.Kind == DeliveryKind.Collective || request.Kind == DeliveryKind.Farm || request.Kind == DeliveryKind.Market)
                    producer.CanDirectSell = true;
                else
                {
                    producer.CanDirectSell = await _context.DeliveryModes.AnyAsync(c => !c.RemovedOn.HasValue && c.Producer.Id == request.RequestUser.Id && (c.Kind == DeliveryKind.Collective || c.Kind == DeliveryKind.Farm || c.Kind == DeliveryKind.Market), token);
                }

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(DeleteDeliveryModeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<DeliveryMode>(request.Id, token);
                var activeAgreements = await _context.Agreements.CountAsync(a => a.Delivery.Id == entity.Id && !a.RemovedOn.HasValue, token);
                if (activeAgreements > 0)
                    return BadRequest<bool>(MessageKind.DeliveryMode_CannotRemove_With_Active_Agreements, entity.Name, activeAgreements);

                _context.Remove(entity);
                entity.Producer.CanDirectSell = await _context.DeliveryModes.AnyAsync(c => !c.RemovedOn.HasValue && c.Producer.Id == request.RequestUser.Id && (c.Kind == DeliveryKind.Collective || c.Kind == DeliveryKind.Farm || c.Kind == DeliveryKind.Market), token);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RestoreDeliveryModeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.DeliveryModes.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);

                _context.Restore(entity);
                entity.Producer.CanDirectSell = await _context.DeliveryModes.AnyAsync(c => !c.RemovedOn.HasValue && c.Producer.Id == request.RequestUser.Id && (c.Kind == DeliveryKind.Collective || c.Kind == DeliveryKind.Farm || c.Kind == DeliveryKind.Market), token);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(SetDeliveryModesAvailabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var id in request.Ids)
                    {
                        var result = await _mediatr.Process(new SetDeliveryModeAvailabilityCommand(request.RequestUser) { Id = id, Available = request.Available }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(SetDeliveryModeAvailabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<DeliveryMode>(request.Id, token);
                entity.SetAvailability(request.Available);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}