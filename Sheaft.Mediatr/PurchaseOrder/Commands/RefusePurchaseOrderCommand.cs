using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.PayinRefund.Commands;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class RefusePurchaseOrderCommand : Command
    {
        protected RefusePurchaseOrderCommand()
        {
            
        }
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
        private readonly ITableService _tableService;

        public RefusePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ITableService tableService,
            ILogger<RefusePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _tableService = tableService;
        }

        public async Task<Result> Handle(RefusePurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == request.PurchaseOrderId, token);
            if(purchaseOrder.ProducerId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            purchaseOrder.Refuse(request.Reason, request.SkipNotification);

            await _context.SaveChangesAsync(token);

            var order = await _context.Orders
                .SingleOrDefaultAsync(o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id), token);
            
            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.ProducerId == purchaseOrder.ProducerId);
            if (delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue)
                await _tableService.DecreaseProducerDeliveryCountAsync(delivery.DeliveryMode.ProducerId,
                    delivery.DeliveryModeId, purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate,
                    purchaseOrder.ExpectedDelivery.From, purchaseOrder.ExpectedDelivery.To,
                    delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.Value, token);
            
            var hasPayins = await _context.Payins.AnyAsync(p =>
                (p.Status == TransactionStatus.Succeeded || p.Status == TransactionStatus.Waiting)
                && p.OrderId == order.Id, token);

            if(hasPayins)
                _mediatr.Schedule(new CreatePayinRefundCommand(request.RequestUser) {PurchaseOrderId = purchaseOrder.Id}, TimeSpan.FromDays(1));
            
            return Success();
        }
    }
}