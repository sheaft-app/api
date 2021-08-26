using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class AcceptPurchaseOrdersCommand : Command
    {
        protected AcceptPurchaseOrdersCommand()
        {
        }

        [JsonConstructor]
        public AcceptPurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
    }

    public class AcceptPurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<AcceptPurchaseOrdersCommand, Result>
    {
        public AcceptPurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<AcceptPurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(AcceptPurchaseOrdersCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var purchaseOrderId in request.PurchaseOrderIds)
                {
                    result =
                        await _mediatr.Process(
                            new AcceptPurchaseOrderCommand(request.RequestUser) {PurchaseOrderId = purchaseOrderId},
                            token);
                    
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