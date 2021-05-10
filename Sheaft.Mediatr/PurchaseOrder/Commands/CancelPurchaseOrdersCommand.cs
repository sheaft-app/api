using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class CancelPurchaseOrdersCommand : Command
    {
        protected CancelPurchaseOrdersCommand()
        {
            
        }
        [JsonConstructor]
        public CancelPurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
        public string Reason { get; set; }
    }

    public class CancelPurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<CancelPurchaseOrdersCommand, Result>
    {
        public CancelPurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CancelPurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CancelPurchaseOrdersCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var purchaseOrderId in request.PurchaseOrderIds)
                {
                    result = await _mediatr.Process(
                        new CancelPurchaseOrderCommand(request.RequestUser)
                            {PurchaseOrderId = purchaseOrderId, Reason = request.Reason}, token);
                    if (!result.Succeeded)
                        break;
                }

                if (result is {Succeeded: false})
                    return result;
                
                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}