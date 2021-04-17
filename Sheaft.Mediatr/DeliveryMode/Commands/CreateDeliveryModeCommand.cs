﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.DeliveryClosing.Commands;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.DeliveryMode.Commands
{
    public class CreateDeliveryModeCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
            ProducerId = requestUser.Id;
        }

        public Guid ProducerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DeliveryKind Kind { get; set; }
        public int? LockOrderHoursBeforeDelivery { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<TimeSlotGroupDto> OpeningHours { get; set; }
        public int? MaxPurchaseOrdersPerTimeSlot { get; set; }
        public bool Available { get; set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; set; }
        public IEnumerable<UpdateOrCreateClosingDto> Closings { get; set; }
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
            var producer = await _context.GetByIdAsync<Domain.Producer>(request.ProducerId, token);

            DeliveryAddress deliveryModeAddress = null;
            if (request.Address != null)
            {
                deliveryModeAddress = new DeliveryAddress(request.Address.Line1, request.Address.Line2,
                    request.Address.Zipcode, request.Address.City, request.Address.Country, request.Address.Longitude,
                    request.Address.Latitude);
            }

            var openingHours = new List<TimeSlotHour>();
            if (request.OpeningHours != null)
            {
                foreach (var oh in request.OpeningHours)
                {
                    openingHours.AddRange(oh.Days.Select(c => new TimeSlotHour(c, oh.From, oh.To)));
                }
            }

            var entity = new Domain.DeliveryMode(Guid.NewGuid(), request.Kind, producer, request.Available,
                deliveryModeAddress, openingHours, request.Name, request.Description);
            entity.SetLockOrderHoursBeforeDelivery(request.LockOrderHoursBeforeDelivery);
            entity.SetAutoAcceptRelatedPurchaseOrders(request.AutoAcceptRelatedPurchaseOrder);
            entity.SetAutoCompleteRelatedPurchaseOrders(request.AutoCompleteRelatedPurchaseOrder);
            entity.SetMaxPurchaseOrdersPerTimeSlot(request.MaxPurchaseOrdersPerTimeSlot);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
            
            var result =
                await _mediatr.Process(
                    new UpdateOrCreateDeliveryClosingsCommand(request.RequestUser)
                        {DeliveryId = entity.Id, Closings = request.Closings}, token);
            if (!result.Succeeded)
                return Failure<Guid>(result);
            
            _mediatr.Post(new UpdateProducerAvailabilityCommand(request.RequestUser) {ProducerId = entity.Producer.Id});
            return Success(entity.Id);
        }
    }
}