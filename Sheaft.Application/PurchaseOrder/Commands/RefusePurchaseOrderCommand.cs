using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Application.PayinRefund.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.PurchaseOrder.Commands
{
    public class RefusePurchaseOrderCommand : Command
    {
        [JsonConstructor]
        public RefusePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
        public string Reason { get; set; }
        public bool SkipNotification { get; set; }
    }

    public class RefusePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<RefusePurchaseOrderCommand, Result>
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

        public async Task<Result> Handle(RefusePurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.PurchaseOrderId, token);
            if(purchaseOrder.Vendor.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            purchaseOrder.Refuse(request.Reason, request.SkipNotification);

            await _context.SaveChangesAsync(token);

            var order = await _context.GetSingleAsync<Domain.Order>(o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id),
                token);
            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == purchaseOrder.Vendor.Id);
            if (delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue)
                await _capingDeliveriesService.DecreaseProducerDeliveryCountAsync(delivery.DeliveryMode.Producer.Id,
                    delivery.DeliveryMode.Id, purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate,
                    purchaseOrder.ExpectedDelivery.From, purchaseOrder.ExpectedDelivery.To,
                    delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.Value, token);

            _mediatr.Schedule(new CreatePayinRefundCommand(request.RequestUser) {PurchaseOrderId = purchaseOrder.Id}, TimeSpan.FromDays(1));
            return Success();
        }
    }
}