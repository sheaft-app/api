using System;
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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Transfer.Commands;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class DeliverPurchaseOrderCommand : Command
    {
        protected DeliverPurchaseOrderCommand()
        {
            
        }
        [JsonConstructor]
        public DeliverPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
            SkipNotification = false;
        }

        public Guid PurchaseOrderId { get; set; }
        public bool SkipNotification { get; set; }
        public string ReceptionedBy { get; set; }
        public string Comment { get; set; }
    }

    public class DeliverPurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<DeliverPurchaseOrderCommand, Result>
    {
        public DeliverPurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<DeliverPurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeliverPurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == request.PurchaseOrderId, token);
            if(purchaseOrder.ProducerId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            if((int)purchaseOrder.ExpectedDelivery.Kind > 4)
                return Failure(MessageKind.Validation);

            purchaseOrder.Delivery.CompleteDelivery(request.ReceptionedBy, request.Comment);
            await _context.SaveChangesAsync(token);

            if (purchaseOrder.SenderInfo.Kind == ProfileKind.Consumer)
            {
                var dateDiff = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate.AddDays(7) - DateTime.UtcNow;
                _mediatr.Schedule(
                    new CreatePurchaseOrderTransferCommand(request.RequestUser) {PurchaseOrderId = purchaseOrder.Id},
                    TimeSpan.FromDays(dateDiff.TotalDays));
            }

            return Success();
        }
    }
}