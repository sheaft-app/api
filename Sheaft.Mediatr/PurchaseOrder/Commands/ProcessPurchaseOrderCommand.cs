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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class ProcessPurchaseOrderCommand : Command
    {
        protected ProcessPurchaseOrderCommand()
        {
            
        }
        [JsonConstructor]
        public ProcessPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
        public bool SkipNotification { get; set; }
    }

    public class ProcessPurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<ProcessPurchaseOrderCommand, Result>
    {
        public ProcessPurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<ProcessPurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(ProcessPurchaseOrderCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == request.PurchaseOrderId, token);
            if(purchaseOrder.ProducerId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            purchaseOrder.Process(request.SkipNotification);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}