using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class AcceptPurchaseOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public AcceptPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool SkipNotification { get; set; }
    }
    
    public class AcceptPurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<AcceptPurchaseOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public AcceptPurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<AcceptPurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }
        
        public async Task<Result<bool>> Handle(AcceptPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Accept();

                await _context.SaveChangesAsync(token);
                
                if(!request.SkipNotification)
                    _mediatr.Post(new PurchaseOrderAcceptedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });

                var order = await _context.GetSingleAsync<Order>(o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id), token);
                var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == purchaseOrder.Vendor.Id);
                if (delivery.DeliveryMode.AutoCompleteRelatedPurchaseOrder)
                    _mediatr.Post(new CompletePurchaseOrderCommand(request.RequestUser) { Id = purchaseOrder.Id, SkipNotification = request.SkipNotification });

                return Ok(true);
            });
        }
    }
}