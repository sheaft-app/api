﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.PayinRefund.Commands;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class CancelPurchaseOrderCommand : Command
    {
        protected CancelPurchaseOrderCommand()
        {
            
        }
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
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == request.PurchaseOrderId, token);
            if (purchaseOrder.ProducerId != request.RequestUser.Id && purchaseOrder.ClientId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            purchaseOrder.Cancel(request.Reason, request.SkipNotification);

            await _context.SaveChangesAsync(token);

            var order = await _context.Orders
                .SingleOrDefaultAsync(o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id), token);
            
            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.ProducerId == purchaseOrder.ProducerId);
            if (delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue)
                await _tableService.DecreaseProducerDeliveryCountAsync(delivery.DeliveryMode.ProducerId,
                    delivery.DeliveryModeId, purchaseOrder.Delivery.ExpectedDeliveryDate,
                    purchaseOrder.Delivery.From, purchaseOrder.Delivery.To,
                    delivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.Value, token);

            var hasPayins = await _context.Payins.AnyAsync(p =>
                (p.Status == TransactionStatus.Succeeded || p.Status == TransactionStatus.Waiting)
                && p.OrderId == order.Id, token);

            if(hasPayins)
                _mediatr.Schedule(new CreatePayinRefundCommand(request.RequestUser) {PurchaseOrderId = purchaseOrder.Id},
                    TimeSpan.FromDays(1));
            
            return Success();
        }
    }
}