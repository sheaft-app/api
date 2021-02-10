using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RestorePurchaseOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public RestorePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class RestorePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<RestorePurchaseOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public RestorePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<RestorePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
        }
        
        public async Task<Result<bool>> Handle(RestorePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.PurchaseOrders.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);

                _context.Restore(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}
