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
    public class RefusePurchaseOrdersCommand : Command
    {
        [JsonConstructor]
        public RefusePurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }

    public class RefusePurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<RefusePurchaseOrdersCommand, Result>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public RefusePurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<RefusePurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }

        public async Task<Result> Handle(RefusePurchaseOrdersCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var errors = new List<string>();
                foreach (var purchaseOrderId in request.Ids)
                {
                    var result = await _mediatr.Process(
                        new RefusePurchaseOrderCommand(request.RequestUser)
                            {Id = purchaseOrderId, Reason = request.Reason}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}