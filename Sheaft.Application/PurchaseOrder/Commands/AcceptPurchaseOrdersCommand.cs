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
    public class AcceptPurchaseOrdersCommand : Command<bool>
    {
        [JsonConstructor]
        public AcceptPurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
    
    public class AcceptPurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<AcceptPurchaseOrdersCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public AcceptPurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<AcceptPurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }
        
        public async Task<Result<bool>> Handle(AcceptPurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var purchaseOrderId in request.Ids)
                    {
                        var result = await _mediatr.Process(new AcceptPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
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