using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class ProcessPurchaseOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public ProcessPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool SkipNotification { get; set; }
    }
    
    public class ProcessPurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<ProcessPurchaseOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public ProcessPurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<ProcessPurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }
        
        public async Task<Result<bool>> Handle(ProcessPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Process();

                await _context.SaveChangesAsync(token);

                if (!request.SkipNotification)
                    _mediatr.Post(new PurchaseOrderProcessingEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });

                return Ok(true);
            });
        }
    }
}