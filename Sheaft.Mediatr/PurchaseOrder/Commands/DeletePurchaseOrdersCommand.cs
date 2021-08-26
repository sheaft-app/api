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
    public class DeletePurchaseOrdersCommand : Command
    {
        protected DeletePurchaseOrdersCommand()
        {
            
        }
        [JsonConstructor]
        public DeletePurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
    }

    public class DeletePurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<DeletePurchaseOrdersCommand, Result>
    {
        public DeletePurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<DeletePurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeletePurchaseOrdersCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var purchaseOrderId in request.PurchaseOrderIds)
                {
                    result =
                        await _mediatr.Process(
                            new DeletePurchaseOrderCommand(request.RequestUser) {PurchaseOrderId = purchaseOrderId}, token);
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