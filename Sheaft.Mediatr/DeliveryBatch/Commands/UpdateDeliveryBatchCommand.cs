using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
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
        private readonly IIdentifierService _identifierService;

        public UpdateDeliveryBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IIdentifierService identifierService,
            ILogger<UpdateDeliveryBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
        }

        public async Task<Result> Handle(UpdateDeliveryBatchCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.Id, token);
            if (deliveryBatch == null)
                return Failure("La tournée de livraison est introuvable.");

            if (deliveryBatch.Status != DeliveryBatchStatus.Waiting)
                return Failure("Impossible de modifier une livraison qui n'est pas en attente.");
            
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
            foreach (var clientDelivery in request.Deliveries.OrderBy(d => d.Position))
            {
                var delivery = deliveryBatch.Deliveries.SingleOrDefault(d => d.ClientId == clientDelivery.ClientId);
                var purchaseOrders = await _context.PurchaseOrders
                    .Where(p => clientDelivery.PurchaseOrderIds.Contains(p.Id))
                    .ToListAsync(token);
                
                if(purchaseOrders.Any(po => po.Status != PurchaseOrderStatus.Completed))
                    throw SheaftException.Validation("Certaines commandes ne sont pas prête.");
                
                if(purchaseOrders.Any(po => (int)po.ExpectedDelivery.Kind <= 4))
                    throw SheaftException.Validation("Certaines commandes doivent être récupérée et non livrée.");

                if (delivery == null)
                {
                    var order = purchaseOrders.First();
                    var user = await _context.Users.SingleAsync(u => u.Id == order.ClientId, token);
                    
                    var identifier = await _identifierService.GetNextDeliveryReferenceAsync(deliveryBatch.AssignedTo.Id, token);
                    if (!identifier.Succeeded)
                        return Failure(identifier);
                        
                    delivery = new Domain.Delivery(identifier.Data, (Domain.Producer)deliveryBatch.AssignedTo,
                        order.ExpectedDelivery.Kind,
                        deliveryBatch.ScheduledOn, order.ExpectedDelivery.Address, user.Id, user.Name, purchaseOrders,
                        currentPosition);

                    deliveryBatch.AddDelivery(delivery);
                }
                else
                {
                    var existingPurchaseOrderIds = delivery.PurchaseOrders.Select(po => po.Id);
                    var newPurchaseOrderIds = purchaseOrders.Select(po => po.Id);
                    var purchaseOrderIdsToRemove = existingPurchaseOrderIds.Except(newPurchaseOrderIds).ToList();
                    if (purchaseOrderIdsToRemove.Any())
                    {
                        var purchaseOrdersToRemove =
                            delivery.PurchaseOrders.Where(po => purchaseOrderIdsToRemove.Contains(po.Id));
                        
                        delivery.RemovePurchaseOrders(purchaseOrdersToRemove);
                    }

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