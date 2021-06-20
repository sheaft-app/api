using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.DeliveryBatch.Commands
{
    public class UpdateDeliveryBatchCommand : Command
    {
        protected UpdateDeliveryBatchCommand()
        {
        }

        [JsonConstructor]
        public UpdateDeliveryBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ClientDeliveryPositionDto> Deliveries { get; set; }
    }

    public class UpdateDeliveryBatchCommandHandler : CommandsHandler,
        IRequestHandler<UpdateDeliveryBatchCommand, Result>
    {
        public UpdateDeliveryBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateDeliveryBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateDeliveryBatchCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.Id, token);
            if (deliveryBatch == null)
                return Failure(MessageKind.NotFound);

            var clientIds = request.Deliveries.Select(d => d.ClientId);
            var existingClientIds = deliveryBatch.Deliveries.Select(d => d.ClientId);

            var deliveriesToRemove = existingClientIds.Except(clientIds);
            foreach (var clientIdToRemove in deliveriesToRemove)
            {
                var delivery = deliveryBatch.Deliveries.Single(d => d.ClientId == clientIdToRemove);
                deliveryBatch.RemoveDelivery(delivery);
                _context.Remove(delivery);
            }

            var currentPosition = 0;
            foreach (var clientDelivery in request.Deliveries.OrderByDescending(d => d.Position))
            {
                var delivery = deliveryBatch.Deliveries.SingleOrDefault(d => d.ClientId == clientDelivery.ClientId);
                var purchaseOrders = await _context.PurchaseOrders
                    .Where(p => clientDelivery.PurchaseOrderIds.Contains(p.Id))
                    .ToListAsync(token);
                
                if(purchaseOrders.Any(po => po.Status != PurchaseOrderStatus.Completed && po.Status != PurchaseOrderStatus.Postponed))
                    throw SheaftException.Validation();
                
                if(purchaseOrders.Any(po => (int)po.ExpectedDelivery.Kind <= 4))
                    throw SheaftException.Validation();

                if (delivery == null)
                {
                    var order = purchaseOrders.First();
                    var user = await _context.Users.SingleAsync(u => u.Id == order.ClientId, token);
                    delivery = new Domain.Delivery((Domain.Producer) deliveryBatch.AssignedTo,
                        order.ExpectedDelivery.Kind,
                        deliveryBatch.ScheduledOn, order.ExpectedDelivery.Address, user.Id, user.Name, purchaseOrders,
                        currentPosition);

                    deliveryBatch.AddDelivery(delivery);
                }
                else
                {
                    foreach (var purchaseOrder in purchaseOrders)
                    {
                        var existingPurchaseOrder =
                            delivery.PurchaseOrders.SingleOrDefault(po => po.Id == purchaseOrder.Id);
                        if (existingPurchaseOrder == null)
                            delivery.AddPurchaseOrders(new[] {purchaseOrder});
                    }
                }

                delivery.SetPosition(currentPosition);
                currentPosition++;
            }

            deliveryBatch.SetName(request.Name);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}