using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class QueueExportPickingOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public QueueExportPickingOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
        public string Name { get; set; }
    }
    public class QueueExportPickingOrderCommandHandler : CommandsHandler,
        IRequestHandler<QueueExportPickingOrderCommand, Result<Guid>>
    {
        private readonly IBlobService _blobsService;

        public QueueExportPickingOrderCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context, 
            IBlobService blobsService,
            ILogger<QueueExportPickingOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobsService = blobsService;
        }

        public async Task<Result<Guid>> Handle(QueueExportPickingOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.RequestUser.Id, token);
                var purchaseOrders = await _context.GetByIdsAsync<PurchaseOrder>(request.PurchaseOrderIds, token);

                var orderIdsToAccept = purchaseOrders.Where(c => c.Status == PurchaseOrderStatus.Waiting).Select(c => c.Id);
                if (orderIdsToAccept.Any())
                {
                    var result = await _mediatr.Process(new AcceptPurchaseOrdersCommand(request.RequestUser) { Ids = orderIdsToAccept }, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);
                }

                var entity = new Job(Guid.NewGuid(), JobKind.ExportPickingOrders, request.Name ?? $"Export bon préparation", producer);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new ExportPickingOrderCommand(request.RequestUser) { JobId = entity.Id, PurchaseOrderIds = request.PurchaseOrderIds });

                return Ok(entity.Id);
            });
        }
    }
}
