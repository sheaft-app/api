using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class DeliverPurchaseOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public DeliverPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class DeliverPurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<DeliverPurchaseOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public DeliverPurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<DeliverPurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }
        
        public async Task<Result<bool>> Handle(DeliverPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Deliver();

                await _context.SaveChangesAsync(token);

                var producerLegals = await _context.GetSingleAsync<BusinessLegal>(c => c.User.Id == purchaseOrder.Vendor.Id, token);
                _mediatr.Schedule(new CreateTransferCommand(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id }, TimeSpan.FromDays(7));

                return Ok(true);
            });
        }
    }
}
