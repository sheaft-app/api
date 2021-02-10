using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.PurchaseOrder.Commands
{
    public class DeliverPurchaseOrdersCommand : Command
    {
        [JsonConstructor]
        public DeliverPurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }

    public class DeliverPurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<DeliverPurchaseOrdersCommand, Result>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public DeliverPurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<DeliverPurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }

        public async Task<Result> Handle(DeliverPurchaseOrdersCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var purchaseOrderId in request.Ids)
                {
                    var result =
                        await _mediatr.Process(
                            new DeliverPurchaseOrderCommand(request.RequestUser) {Id = purchaseOrderId}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}