using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.PurchaseOrder.Commands
{
    public class CompletePurchaseOrdersCommand : Command
    {
        [JsonConstructor]
        public CompletePurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
    }

    public class CompletePurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<CompletePurchaseOrdersCommand, Result>
    {
        public CompletePurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CompletePurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CompletePurchaseOrdersCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var purchaseOrderId in request.PurchaseOrderIds)
                {
                    var result =
                        await _mediatr.Process(
                            new CompletePurchaseOrderCommand(request.RequestUser) {PurchaseOrderId = purchaseOrderId}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}