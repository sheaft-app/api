using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class AcceptPurchaseOrderCommand : Command
    {
        [JsonConstructor]
        public AcceptPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
        public bool SkipNotification { get; set; }
    }

    public class AcceptPurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<AcceptPurchaseOrderCommand, Result>
    {
        public AcceptPurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<AcceptPurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(AcceptPurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.PurchaseOrderId, token);
            if(purchaseOrder.Vendor.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            purchaseOrder.Accept(request.SkipNotification);

            await _context.SaveChangesAsync(token);

            var order = await _context.GetSingleAsync<Domain.Order>(
                o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id),
                token);
            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == purchaseOrder.Vendor.Id);
            if (delivery.DeliveryMode.AutoCompleteRelatedPurchaseOrder)
                _mediatr.Post(new CompletePurchaseOrderCommand(request.RequestUser)
                    {PurchaseOrderId = purchaseOrder.Id, SkipNotification = request.SkipNotification});

            return Success();
        }
    }
}