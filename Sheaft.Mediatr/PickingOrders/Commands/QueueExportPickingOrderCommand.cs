using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.Mediatr.PickingOrders.Commands
{
    public class QueueExportPickingOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public QueueExportPickingOrderCommand(RequestUser requestUser) : base(requestUser)
        {
            ProducerId = requestUser.Id;
        }

        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
        public string Name { get; set; }
        public Guid ProducerId { get; set; }
    }

    public class QueueExportPickingOrderCommandHandler : CommandsHandler,
        IRequestHandler<QueueExportPickingOrderCommand, Result<Guid>>
    {
        public QueueExportPickingOrderCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<QueueExportPickingOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(QueueExportPickingOrderCommand request, CancellationToken token)
        {
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);
            var purchaseOrders = await _context.PurchaseOrders.Where(d => request.PurchaseOrderIds.Contains(d.Id))
                .ToListAsync(token);

            var orderIdsToAccept = purchaseOrders
                .Where(c => c.Status == PurchaseOrderStatus.Waiting)
                .Select(c => c.Id);
            
            if (orderIdsToAccept.Any())
            {
                var result =
                    await _mediatr.Process(
                        new AcceptPurchaseOrdersCommand(request.RequestUser) {PurchaseOrderIds = orderIdsToAccept},
                        token);
                if (!result.Succeeded)
                    return Failure<Guid>(result);
            }

            var entity = new Domain.Job(Guid.NewGuid(), JobKind.ExportPickingOrders,
                request.Name ?? $"Export bon préparation",
                producer);

            await _context.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(new ExportPickingOrderCommand(request.RequestUser)
                {JobId = entity.Id, PurchaseOrderIds = request.PurchaseOrderIds});

            return Success(entity.Id);
        }
    }
}