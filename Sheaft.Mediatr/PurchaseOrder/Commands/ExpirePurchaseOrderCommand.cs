using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class ExpirePurchaseOrderCommand : Command
    {
        [JsonConstructor]
        public ExpirePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }

    public class ExpirePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<ExpirePurchaseOrderCommand, Result>
    {
        public ExpirePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<ExpirePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ExpirePurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.PurchaseOrderId, token);
            if (purchaseOrder.Status == PurchaseOrderStatus.Waiting 
                && purchaseOrder.CreatedOn.AddDays(3) < DateTimeOffset.UtcNow)
            {
                purchaseOrder.Expire(
                    "La commande n'a pas été acceptée dans le délai de 72 heures prévu par nos conditions d'utilisations.",
                    false);
            }
            
            if (purchaseOrder.Status == PurchaseOrderStatus.Waiting 
                && purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate < DateTimeOffset.UtcNow)
            {
                purchaseOrder.Expire(
                    "La commande n'a pas été acceptée avant la date de réception choisie.",
                    false);
            }

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}