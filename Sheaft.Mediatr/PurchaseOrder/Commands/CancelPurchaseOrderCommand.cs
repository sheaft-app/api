﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.PayinRefund.Commands;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
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
        private readonly ITableService _tableService;

        public CancelPurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ITableService tableService,
            ILogger<CancelPurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _tableService = tableService;
        }

        public async Task<Result> Handle(CancelPurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.PurchaseOrderId, token);
            if (purchaseOrder.Vendor.Id != request.RequestUser.Id && purchaseOrder.Sender.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            purchaseOrder.Cancel(request.Reason, request.SkipNotification);

            await _context.SaveChangesAsync(token);

            var order = await _context.GetSingleAsync<Domain.Order>(
                o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id),
                token);
            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == purchaseOrder.Vendor.Id);
            if (delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue)
                await _tableService.DecreaseProducerDeliveryCountAsync(delivery.DeliveryMode.Producer.Id,
                    delivery.DeliveryMode.Id, purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate,
                    purchaseOrder.ExpectedDelivery.From, purchaseOrder.ExpectedDelivery.To,
                    delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.Value, token);

            var hasPayins = await _context.Payins.AnyAsync(p =>
                (p.Status == TransactionStatus.Succeeded || p.Status == TransactionStatus.Waiting)
                & p.Order.Id == order.Id, token);

            if(hasPayins)
                _mediatr.Schedule(new CreatePayinRefundCommand(request.RequestUser) {PurchaseOrderId = purchaseOrder.Id},
                    TimeSpan.FromDays(1));
            
            return Success();
        }
    }
}