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
    public class WithdrawnPurchaseOrdersCommand : Command
    {
        protected WithdrawnPurchaseOrdersCommand()
        {
            
        }
        [JsonConstructor]
        public WithdrawnPurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
        public string Reason { get; set; }
    }

    public class WithdrawnPurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<WithdrawnPurchaseOrdersCommand, Result>
    {
        public WithdrawnPurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<WithdrawnPurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(WithdrawnPurchaseOrdersCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var purchaseOrderId in request.PurchaseOrderIds)
                {
                    result = await _mediatr.Process(
                        new WithdrawnPurchaseOrderCommand(request.RequestUser)
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