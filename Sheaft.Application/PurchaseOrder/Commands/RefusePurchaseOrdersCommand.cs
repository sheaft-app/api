using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RefusePurchaseOrdersCommand : Command<bool>
    {
        [JsonConstructor]
        public RefusePurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
    
    public class RefusePurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<RefusePurchaseOrdersCommand, Result<bool>>
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
        
        public async Task<Result<bool>> Handle(RefusePurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var errors = new List<string>();
                    foreach (var purchaseOrderId in request.Ids)
                    {
                        var result = await _mediatr.Process(new RefusePurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId, Reason = request.Reason }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }
    }
}