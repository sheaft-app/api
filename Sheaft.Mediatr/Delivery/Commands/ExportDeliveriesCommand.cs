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
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Domain.Events.PurchaseOrder;

namespace Sheaft.Mediatr.Delivery.Commands
{
    public class ExportDeliveriesCommand : Command
    {
        protected ExportDeliveriesCommand()
        {
            
        }
        [JsonConstructor]
        public ExportDeliveriesCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public IEnumerable<DeliveryKind> Kinds { get; set; }
    }

    public class ExportDeliveriesCommandHandler : CommandsHandler,
        IRequestHandler<ExportDeliveriesCommand, Result>
    {
        private readonly IBlobService _blobService;
        private readonly IDeliveriesExportersFactory _deliveriesExportersFactory;

        public ExportDeliveriesCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            IDeliveriesExportersFactory deliveriesExportersFactory,
            ILogger<ExportDeliveriesCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
            _deliveriesExportersFactory = deliveriesExportersFactory;
        }

        public async Task<Result> Handle(ExportDeliveriesCommand request, CancellationToken token)
        {
            var job = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if (job.UserId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            try
            {
                job.StartJob(new DeliveriesExportProcessingEvent(job.Id));
                await _context.SaveChangesAsync(token);

                var deliveriesQuery = _context.Set<Domain.Delivery>().Where(o =>
                    o.ProducerId == request.RequestUser.Id
                    && o.Status == DeliveryStatus.Delivered
                    && request.Kinds.Contains(o.Kind)
                    && o.DeliveredOn >= request.From
                    && o.DeliveredOn <= request.To);

                var exporter = await _deliveriesExportersFactory.GetExporterAsync(request.RequestUser, token);
                var deliveriesExportResult = await exporter.ExportAsync(request.RequestUser, request.From, request.To,
                    deliveriesQuery, token);

                if (!deliveriesExportResult.Succeeded)
                    throw deliveriesExportResult.Exception;

                var response = await _blobService.UploadUserDeliveriesFileAsync(job.UserId, job.Id,
                    $"Ventes_Magasins_du_{request.From:dd-MM-yyyy}_au_{request.To:dd-MM-yyyy}.{deliveriesExportResult.Data.Extension}", deliveriesExportResult.Data.Data, token);
                if (!response.Succeeded)
                    throw response.Exception;

                job.SetDownloadUrl(response.Data);
                job.CompleteJob(new DeliveriesExportSucceededEvent(job.Id));

                await _context.SaveChangesAsync(token);
                return Success();
            }
            catch (Exception e)
            {
                job.FailJob(e.Message, new DeliveriesExportFailedEvent(job.Id));
                await _context.SaveChangesAsync(token);
                
                return Failure("Une erreur est survenue pendant l'export des livraisons.");
            }
        }
    }
}