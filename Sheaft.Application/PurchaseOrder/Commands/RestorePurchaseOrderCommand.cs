using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class RestorePurchaseOrderCommand : Command
    {
        [JsonConstructor]
        public RestorePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }

    public class RestorePurchaseOrderCommandHandler : CommandsHandler,
        IRequestHandler<RestorePurchaseOrderCommand, Result>
    {
        public RestorePurchaseOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<RestorePurchaseOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestorePurchaseOrderCommand request, CancellationToken token)
        {
            var entity =
                await _context.PurchaseOrders.SingleOrDefaultAsync(a => a.Id == request.PurchaseOrderId && a.RemovedOn.HasValue,
                    token);

            _context.Restore(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}