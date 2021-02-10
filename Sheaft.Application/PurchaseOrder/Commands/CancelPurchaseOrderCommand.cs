using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.PayinRefund.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Events.PurchaseOrder;

namespace Sheaft.Application.PurchaseOrder.Commands
{
    public class CancelPurchaseOrderCommand : Command
    {
        [JsonConstructor]
        public CancelPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
        public string Reason { get; set; }
        public bool SkipNotification { get; set; }
    }

    public class CancelPurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<CancelPurchaseOrderCommand, Result>
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

        public async Task<Result> Handle(CancelPurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.PurchaseOrderId, token);
            purchaseOrder.Cancel(request.Reason, request.SkipNotification);

            await _context.SaveChangesAsync(token);

            var order = await _context.GetSingleAsync<Domain.Order>(o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id),
                token);
            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == purchaseOrder.Vendor.Id);
            if (delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue)
                await _capingDeliveriesService.DecreaseProducerDeliveryCountAsync(delivery.DeliveryMode.Producer.Id,
                    delivery.DeliveryMode.Id, purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate,
                    purchaseOrder.ExpectedDelivery.From, purchaseOrder.ExpectedDelivery.To,
                    delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.Value, token);

            _mediatr.Schedule(new CreatePayinRefundCommand(request.RequestUser) {PurchaseOrderId = purchaseOrder.Id},
                TimeSpan.FromDays(1));
            return Success();
        }
    }
}