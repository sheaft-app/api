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
    public class CompletePurchaseOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public CompletePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool SkipNotification { get; set; }
    }
    
    public class CompletePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<CompletePurchaseOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public CompletePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<CompletePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }
        
        public async Task<Result<bool>> Handle(CompletePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Complete();

                await _context.SaveChangesAsync(token);

                if (!request.SkipNotification)
                    _mediatr.Post(new PurchaseOrderCompletedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });

                return Ok(true);
            });
        }
    }
}