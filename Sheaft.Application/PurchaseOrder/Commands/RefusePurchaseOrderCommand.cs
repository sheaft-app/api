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
    public class RefusePurchaseOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public RefusePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
        public bool SkipNotification { get; set; }
    }
    
    public class RefusePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<RefusePurchaseOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public RefusePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<RefusePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }
        
        public async Task<Result<bool>> Handle(RefusePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Refuse(request.Reason);

                await _context.SaveChangesAsync(token);

                var order = await _context.GetSingleAsync<Order>(o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id), token);
                var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == purchaseOrder.Vendor.Id);
                if (delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue)
                    await _capingDeliveriesService.DecreaseProducerDeliveryCountAsync(delivery.DeliveryMode.Producer.Id, delivery.DeliveryMode.Id, purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate, purchaseOrder.ExpectedDelivery.From, purchaseOrder.ExpectedDelivery.To, delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.Value, token);

                if (!request.SkipNotification)
                    _mediatr.Post(new PurchaseOrderRefusedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });

                _mediatr.Schedule(new CreatePayinRefundCommand(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id }, TimeSpan.FromDays(1));
                return Ok(true);
            });
        }
    }
}