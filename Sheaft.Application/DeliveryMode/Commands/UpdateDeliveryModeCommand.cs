using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Models;
using Sheaft.Domain.Enums;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class UpdateDeliveryModeCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DeliveryKind Kind { get; set; }
        public string Description { get; set; }
        public int? MaxPurchaseOrdersPerTimeSlot { get; set; }
        public int? LockOrderHoursBeforeDelivery { get; set; }
        public LocationAddressInput Address { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
        public bool Available { get; set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; set; }
    }
    public class UpdateDeliveryModeCommandHandler : CommandsHandler,
        IRequestHandler<UpdateDeliveryModeCommand, Result<bool>>
    {
        public UpdateDeliveryModeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateDeliveryModeCommandHandler> logger)
            : base(mediatr, context, logger)
        {
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
                entity.SetMaxPurchaseOrdersPerTimeSlot(request.MaxPurchaseOrdersPerTimeSlot);

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
    }
}
