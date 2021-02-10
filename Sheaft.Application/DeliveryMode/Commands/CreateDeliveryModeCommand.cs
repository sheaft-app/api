﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Models;
using Sheaft.Domain.Enums;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class CreateDeliveryModeCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DeliveryKind Kind { get; set; }
        public int? LockOrderHoursBeforeDelivery { get; set; }
        public LocationAddressInput Address { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
        public int? MaxPurchaseOrdersPerTimeSlot { get; set; }
        public bool Available { get; set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; set; }
    }
    
    public class CreateDeliveryModeCommandHandler : CommandsHandler,
        IRequestHandler<CreateDeliveryModeCommand, Result<Guid>>
    {
        public CreateDeliveryModeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateDeliveryModeCommandHandler> logger)
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

                var entity = new DeliveryMode(Guid.NewGuid(), request.Kind, producer, request.Available, deliveryModeAddress, openingHours, request.Name, request.Description);
                entity.SetLockOrderHoursBeforeDelivery(request.LockOrderHoursBeforeDelivery);
                entity.SetAutoAcceptRelatedPurchaseOrders(request.AutoAcceptRelatedPurchaseOrder);
                entity.SetAutoCompleteRelatedPurchaseOrders(request.AutoCompleteRelatedPurchaseOrder);
                entity.SetMaxPurchaseOrdersPerTimeSlot(request.MaxPurchaseOrdersPerTimeSlot);

                if (request.Kind == DeliveryKind.Collective || request.Kind == DeliveryKind.Farm || request.Kind == DeliveryKind.Market)
                    producer.CanDirectSell = true;

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                return Ok(entity.Id);
            });
        }
    }
}
