using System;
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

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class RestorePurchaseOrderCommand : Command
    {
        protected RestorePurchaseOrderCommand()
        {
            
        }
        [JsonConstructor]
        public RestorePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }

    public class RestorePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<RestorePurchaseOrderCommand, Result>
    {
        public RestorePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<RestorePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestorePurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder =
                await _context.PurchaseOrders.SingleOrDefaultAsync(a => a.Id == request.PurchaseOrderId && a.RemovedOn.HasValue,
                    token);
            if(purchaseOrder.ProducerId != request.RequestUser.Id && purchaseOrder.ClientId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            _context.Restore(purchaseOrder);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}