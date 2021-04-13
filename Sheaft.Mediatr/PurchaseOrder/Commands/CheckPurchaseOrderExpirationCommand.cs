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
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.Mediatr.Payin.Commands
{
    public class CheckPurchaseOrderExpirationCommand : Command
    {
        [JsonConstructor]
        public CheckPurchaseOrderExpirationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }

    public class CheckPurchaseOrderExpirationCommandHandler : CommandsHandler,
        IRequestHandler<CheckPurchaseOrderExpirationCommand, Result>
    {
        public CheckPurchaseOrderExpirationCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckPurchaseOrderExpirationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckPurchaseOrderExpirationCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.PurchaseOrderId, token);
            if (purchaseOrder.Status == PurchaseOrderStatus.Waiting 
                && purchaseOrder.Sender.Kind == ProfileKind.Consumer 
                && purchaseOrder.CreatedOn.AddDays(3) < DateTimeOffset.UtcNow)
            {
                return await _mediatr.Process(
                    new CancelPurchaseOrderCommand(request.RequestUser)
                        {PurchaseOrderId = purchaseOrder.Id, Reason = "La commande n'a pas été acceptée dans le délai de 72 heures prévu par nos conditions d'utilisations.", SkipNotification = false}, token);
            }
            
            if (purchaseOrder.Status == PurchaseOrderStatus.Waiting 
                && purchaseOrder.Sender.Kind == ProfileKind.Consumer 
                && purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate < DateTimeOffset.UtcNow)
            {
                return await _mediatr.Process(
                        new CancelPurchaseOrderCommand(request.RequestUser)
                            {PurchaseOrderId = purchaseOrder.Id, Reason = "La commande n'a pas été acceptée avant la date de réception choisie.", SkipNotification = false}, token);
            }
            
            return Success();
        }
    }
}