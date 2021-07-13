using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Events.PickingOrder;
using Sheaft.Mediatr.Job.Commands;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.Mediatr.PickingOrder.Commands
{
    public class ExportPickingOrderCommand : Command
    {
        protected ExportPickingOrderCommand()
        {
            
        }
        [JsonConstructor]
        public ExportPickingOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
    }

    public class ExportPickingOrderCommandHandler : CommandsHandler,
        IRequestHandler<ExportPickingOrderCommand, Result>
    {
        private readonly IBlobService _blobsService;
        private readonly IPickingOrdersExportersFactory _pickingOrdersExportersFactory;

        public ExportPickingOrderCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobsService,
            IPickingOrdersExportersFactory pickingOrdersExportersFactory,
            ILogger<ExportPickingOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobsService = blobsService;
            _pickingOrdersExportersFactory = pickingOrdersExportersFactory;
        }

        public async Task<Result> Handle(ExportPickingOrderCommand request, CancellationToken token)
        {
            var job = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);

            try
            {
                job.StartJob(new PickingOrderExportProcessingEvent(job.Id));
                await _context.SaveChangesAsync(token);
                
                var exporter = await _pickingOrdersExportersFactory.GetExporterAsync(request.RequestUser, token);
                
                var purchaseOrdersQuery = _context.PurchaseOrders
                    .Where(po => request.PurchaseOrderIds.Contains(po.Id));
                var pickingOrdersExportResult = await exporter.ExportAsync(request.RequestUser, purchaseOrdersQuery, token);
                if (!pickingOrdersExportResult.Succeeded)
                    throw pickingOrdersExportResult.Exception;

                var response = await _blobsService.UploadPickingOrderFileAsync(job.UserId,
                    $"Preparation_{job.CreatedOn:dd-MM-yyyy}.{pickingOrdersExportResult.Data.Extension}",
                    pickingOrdersExportResult.Data.Data, token);
                if (!response.Succeeded)
                    throw response.Exception;

                var result = await _mediatr.Process(
                    new ProcessPurchaseOrdersCommand(request.RequestUser) {PurchaseOrderIds = request.PurchaseOrderIds},
                    token);
                if (!result.Succeeded)
                    throw result.Exception;
                
                job.SetDownloadUrl(response.Data);
                job.CompleteJob(new PickingOrderExportSucceededEvent(job.Id));

                await _context.SaveChangesAsync(token);
                return Success();
            }
            catch (Exception e)
            {
                job.FailJob(e.Message, new PickingOrderExportFailedEvent(job.Id));
                await _context.SaveChangesAsync(token);
                
                return Failure("Une erreur est survenue pendant le traitement de la tâche.");
            }
        }
    }
}