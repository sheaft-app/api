using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Services.PurchaseOrder.Commands
{
    public class RefusePurchaseOrdersCommand : Command
    {
        [JsonConstructor]
        public RefusePurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
        public string Reason { get; set; }
    }

    public class RefusePurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<RefusePurchaseOrdersCommand, Result>
    {
        public RefusePurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<RefusePurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RefusePurchaseOrdersCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var errors = new List<string>();
                foreach (var purchaseOrderId in request.PurchaseOrderIds)
                {
                    var result = await _mediatr.Process(
                        new RefusePurchaseOrderCommand(request.RequestUser)
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