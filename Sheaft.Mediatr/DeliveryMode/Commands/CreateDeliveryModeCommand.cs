using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Mediatr.DeliveryClosing.Commands;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.DeliveryMode.Commands
{
    public class CreateDeliveryModeCommand : Command<Guid>
    {
        protected CreateDeliveryModeCommand()
        {
            
        }
        [JsonConstructor]
        public CreateDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
            ProducerId = RequestUser.Id;
        }

        public Guid ProducerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DeliveryKind Kind { get; set; }
        public int? LockOrderHoursBeforeDelivery { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<TimeSlotGroupDto> DeliveryHours { get; set; }
        public int? MaxPurchaseOrdersPerTimeSlot { get; set; }
        public bool Available { get; set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; set; }
        public IEnumerable<ClosingInputDto> Closings { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            ProducerId = RequestUser.Id;
        }
    }

    public class CreateDeliveryModeCommandHandler : CommandsHandler,
        IRequestHandler<CreateDeliveryModeCommand, Result<Guid>>
    {
        public CreateDeliveryModeCommandHandler(
            ISheaftMediatr mediatr,
            IDbContextFactory<AppDbContext> context,
            ILogger<CreateDeliveryModeCommandHandler> logger)
            : base(mediatr, context.CreateDbContext(), logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateDeliveryModeCommand request, CancellationToken token)
        {
            DeliveryAddress deliveryModeAddress = null;
            if (request.Address != null)
            {
                deliveryModeAddress = new DeliveryAddress(request.Address.Line1, request.Address.Line2,
                    request.Address.Zipcode, request.Address.City, request.Address.Country, request.Address.Longitude,
                    request.Address.Latitude);
            }

            var openingHours = new List<DeliveryHours>();
            if (request.DeliveryHours != null)
                foreach (var oh in request.DeliveryHours)
                    openingHours.AddRange(oh.Days.Select(c => new DeliveryHours(c, oh.From, oh.To)));
            
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);
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