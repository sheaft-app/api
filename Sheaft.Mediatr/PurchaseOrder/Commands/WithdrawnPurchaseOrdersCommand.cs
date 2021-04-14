using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class WithdrawnPurchaseOrdersCommand : Command
    {
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
                foreach (var purchaseOrderId in request.PurchaseOrderIds)
                {
                    var result = await _mediatr.Process(
                        new WithdrawnPurchaseOrderCommand(request.RequestUser)
                            {PurchaseOrderId = purchaseOrderId, Reason = request.Reason}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}