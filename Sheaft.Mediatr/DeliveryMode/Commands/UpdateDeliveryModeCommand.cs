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
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.DeliveryClosing.Commands;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.DeliveryMode.Commands
{
    public class UpdateDeliveryModeCommand : Command
    {
        protected UpdateDeliveryModeCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryModeId { get; set; }
        public string Name { get; set; }
        public DeliveryKind Kind { get; set; }
        public string Description { get; set; }
        public int? MaxPurchaseOrdersPerTimeSlot { get; set; }
        public int? LockOrderHoursBeforeDelivery { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<TimeSlotGroupDto> OpeningHours { get; set; }
        public bool Available { get; set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; set; }
        public IEnumerable<ClosingInputDto> Closings { get; set; }
    }

    public class UpdateDeliveryModeCommandHandler : CommandsHandler,
        IRequestHandler<UpdateDeliveryModeCommand, Result>
    {
        public UpdateDeliveryModeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateDeliveryModeCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateDeliveryModeCommand request, CancellationToken token)
        {
            var entity = await _context.DeliveryModes.SingleAsync(e => e.Id == request.DeliveryModeId, token);
            if(entity.ProducerId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            entity.SetName(request.Name);
            entity.SetAvailability(request.Available);
            entity.SetDescription(request.Description);
            entity.SetLockOrderHoursBeforeDelivery(request.LockOrderHoursBeforeDelivery);
            entity.SetKind(request.Kind);
            entity.SetAutoAcceptRelatedPurchaseOrders(request.AutoAcceptRelatedPurchaseOrder);
            entity.SetAutoCompleteRelatedPurchaseOrders(request.AutoCompleteRelatedPurchaseOrder);
            entity.SetMaxPurchaseOrdersPerTimeSlot(request.MaxPurchaseOrdersPerTimeSlot);

            if (request.Address != null)
                entity.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country, request.Address.Longitude, request.Address.Latitude);

            if (request.OpeningHours != null)
            {
                var openingHours = new List<DeliveryHours>();
                foreach (var oh in request.OpeningHours)
                    openingHours.AddRange(oh.Days.Select(c => new DeliveryHours(c, oh.From, oh.To)));

                entity.SetDeliveryHours(openingHours);
            }

            await _context.SaveChangesAsync(token);
            
            var result =
                await _mediatr.Process(
                    new UpdateOrCreateDeliveryClosingsCommand(request.RequestUser)
                        {DeliveryId = entity.Id, Closings = request.Closings}, token);
            if (!result.Succeeded)
                return Failure(result);
            
            _mediatr.Post(new UpdateProducerAvailabilityCommand(request.RequestUser) {ProducerId = entity.ProducerId});
            return Success();
        }
    }
}