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
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
        public IEnumerable<TimeSlotGroupDto> DeliveryHours { get; set; }
        public bool Available { get; set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; set; }
        public IEnumerable<ClosingInputDto> Closings { get; set; }
        public IEnumerable<AgreementPositionDto> Agreements { get; set; }
        public decimal? AcceptPurchaseOrdersWithAmountGreaterThan { get; set; }
        public DeliveryFeesApplication ApplyDeliveryFeesWhen { get; set; }
        public decimal? DeliveryFees { get; set; }
        public decimal? DeliveryFeesMinPurchaseOrderAmount { get; set; }
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
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            entity.SetName(request.Name);
            entity.SetAvailability(request.Available);
            entity.SetDescription(request.Description);
            entity.SetLockOrderHoursBeforeDelivery(request.LockOrderHoursBeforeDelivery);
            entity.SetKind(request.Kind);
            entity.SetAutoAcceptRelatedPurchaseOrders(request.AutoAcceptRelatedPurchaseOrder);
            entity.SetAutoCompleteRelatedPurchaseOrders(request.AutoCompleteRelatedPurchaseOrder);
            entity.SetMaxPurchaseOrdersPerTimeSlot(request.MaxPurchaseOrdersPerTimeSlot);
            entity.SetAcceptPurchaseOrdersWithAmountGreaterThan(request.AcceptPurchaseOrdersWithAmountGreaterThan);
            entity.SetDeliveryFees(request.ApplyDeliveryFeesWhen, request.DeliveryFees, request.DeliveryFeesMinPurchaseOrderAmount);

            if (request.Address != null)
                entity.SetAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City, request.Address.Country, request.Address.Longitude, request.Address.Latitude);

            if (request.DeliveryHours != null)
            {
                var openingHours = new List<DeliveryHours>();
                foreach (var oh in request.DeliveryHours)
                    openingHours.AddRange(oh.Days.Select(c => new DeliveryHours(c, oh.From, oh.To)));

                entity.SetDeliveryHours(openingHours);
            }

            if (request.Agreements != null && request.Agreements.Any())
            {
                if (request.Agreements.Select(c => c.Id).Distinct().Count() != entity.Agreements.Select(c => c.Id).Distinct().Count())
                    return Failure("Vous ne pouvez pas modifier les partenariats rattachés à ce mode de livraison, vous devez directement mettre à jour le partenariat.");

                var positionCount = 0;
                foreach (var requestAgreement in request.Agreements.OrderBy(a => a.Position))
                {
                    var agreement = entity.Agreements.Single(a => a.Id == requestAgreement.Id);
                    agreement.SetPosition(positionCount);
                    positionCount++;
                }
            }

            if (entity.Kind is DeliveryKind.Collective or DeliveryKind.Farm or DeliveryKind.Market)
                entity.Producer.SetCanDirectSell(true);
            
            await _context.SaveChangesAsync(token);
            
            var result =
                await _mediatr.Process(
                    new UpdateOrCreateDeliveryClosingsCommand(request.RequestUser)
                        {DeliveryId = entity.Id, Closings = request.Closings}, token);
            
            if (!result.Succeeded)
                return Failure(result);

            return Success();
        }
    }
}