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
    public class CancelPurchaseOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public CancelPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
        public bool SkipNotification { get; set; }
    }
    
    public class CancelPurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<CancelPurchaseOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public CancelPurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<CancelPurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }

        public async Task<Result<bool>> Handle(CancelPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Cancel(request.Reason);

                await _context.SaveChangesAsync(token);

                var order = await _context.GetSingleAsync<Order>(o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id), token);
                var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == purchaseOrder.Vendor.Id);
                if (delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue)
                    await _capingDeliveriesService.DecreaseProducerDeliveryCountAsync(delivery.DeliveryMode.Producer.Id, delivery.DeliveryMode.Id, purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate, purchaseOrder.ExpectedDelivery.From, purchaseOrder.ExpectedDelivery.To, delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.Value, token);

                if (!request.SkipNotification)
                {
                    if (request.RequestUser.Id == purchaseOrder.Sender.Id)
                        _mediatr.Post(new PurchaseOrderWithdrawnEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });
                    else
                        _mediatr.Post(new PurchaseOrderCancelledEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });
                }

                _mediatr.Schedule(new CreatePayinRefundCommand(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id }, TimeSpan.FromDays(1));
                return Ok(true);
            });
        }
    }
}