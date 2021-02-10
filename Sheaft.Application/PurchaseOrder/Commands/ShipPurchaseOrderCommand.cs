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
    public class ShipPurchaseOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public ShipPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class ShipPurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<ShipPurchaseOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public ShipPurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<ShipPurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }
        
        public async Task<Result<bool>> Handle(ShipPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Ship();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
