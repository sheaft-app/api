using System;
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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class AcceptPurchaseOrderCommand : Command
    {
        protected AcceptPurchaseOrderCommand()
        {
            
        }
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
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == request.PurchaseOrderId, token);
            if(purchaseOrder.ProducerId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            purchaseOrder.Accept(request.SkipNotification);

            await _context.SaveChangesAsync(token);

            var order = await _context.Orders.SingleAsync(
                o => o.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id),
                token);
            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.ProducerId == purchaseOrder.ProducerId);
            if (delivery.DeliveryMode.AutoCompleteRelatedPurchaseOrder)
                _mediatr.Post(new CompletePurchaseOrderCommand(request.RequestUser)
                    {PurchaseOrderId = purchaseOrder.Id, SkipNotification = request.SkipNotification});

            return Success();
        }
    }
}