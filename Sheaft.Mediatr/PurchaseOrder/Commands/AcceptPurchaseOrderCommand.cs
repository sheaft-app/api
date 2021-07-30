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
using Sheaft.Mediatr.Picking.Commands;

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

            var deliveryMode = await
                _context.DeliveryModes.SingleAsync(d => d.Id == purchaseOrder.ExpectedDelivery.DeliveryModeId, token);

            if (deliveryMode.AutoCompleteRelatedPurchaseOrder)
            {
                var result =
                    await _mediatr.Process(
                        new CreatePickingCommand(request.RequestUser)
                            {AutoStart = true, PurchaseOrderIds = new[] {purchaseOrder.Id}}, token);
                
                if (!result.Succeeded)
                    return Success();
                
                var completeResult =
                    await _mediatr.Process(
                        new CompletePickingCommand(request.RequestUser)
                            {PickingId = result.Data, AutoComplete = true}, token);
                
                if (!completeResult.Succeeded)
                    return Success();
            }

            return Success();
        }
    }
}