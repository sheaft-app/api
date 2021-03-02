using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.PurchaseOrder.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.PickingOrders.Commands
{
    public class QueueExportPickingOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public QueueExportPickingOrderCommand(RequestUser requestUser) : base(requestUser)
        {
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
            var producer = await _context.GetByIdAsync<Domain.Producer>(request.ProducerId, token);
            var purchaseOrders = await _context.GetByIdsAsync<Domain.PurchaseOrder>(request.PurchaseOrderIds, token);

            var orderIdsToAccept = purchaseOrders.Where(c => c.Status == PurchaseOrderStatus.Waiting).Select(c => c.Id);
            if (orderIdsToAccept.Any())
            {
                var result =
                    await _mediatr.Process(
                        new AcceptPurchaseOrdersCommand(request.RequestUser) {PurchaseOrderIds = orderIdsToAccept}, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result.Exception);
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