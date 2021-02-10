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
    public class ProcessPurchaseOrdersCommand : Command<bool>
    {
        [JsonConstructor]
        public ProcessPurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
    
    public class ProcessPurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<ProcessPurchaseOrdersCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public ProcessPurchaseOrdersCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<ProcessPurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }
        
        public async Task<Result<bool>> Handle(ProcessPurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var purchaseOrderId in request.Ids)
                    {
                        var result = await _mediatr.Process(new ProcessPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
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