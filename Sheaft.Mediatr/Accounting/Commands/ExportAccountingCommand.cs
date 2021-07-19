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

namespace Sheaft.Mediatr.Accounting.Commands
{
    public class ExportAccountingCommand : Command
    {
        protected ExportAccountingCommand()
        {
            
        }
        [JsonConstructor]
        public ExportAccountingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public IEnumerable<DeliveryKind> Kinds { get; set; }
    }

    public class ExportAccountingCommandHandler : CommandsHandler,
        IRequestHandler<ExportAccountingCommand, Result>
    {
        private readonly IBlobService _blobService;
        private readonly IAccountingExportersFactory _accountingExportersFactory;

        public ExportAccountingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            IAccountingExportersFactory accountingExportersFactory,
            ILogger<ExportAccountingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
            _accountingExportersFactory = accountingExportersFactory;
        }

        public async Task<Result> Handle(ExportAccountingCommand request, CancellationToken token)
        {
            var job = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if (job.UserId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            try
            {
                job.StartJob(new AccountingExportProcessingEvent(job.Id));
                await _context.SaveChangesAsync(token);

                var deliveriesQuery = _context.Set<Domain.Delivery>().Where(o =>
                    o.ProducerId == request.RequestUser.Id
                    && o.Status == DeliveryStatus.Delivered
                    && request.Kinds.Contains(o.Kind)
                    && o.DeliveredOn >= request.From
                    && o.DeliveredOn <= request.To);

                var exporter = await _accountingExportersFactory.GetExporterAsync(request.RequestUser, token);
                var deliveriesExportResult = await exporter.ExportAsync(request.RequestUser, request.From, request.To,
                    deliveriesQuery, token);

                if (!deliveriesExportResult.Succeeded)
                    throw deliveriesExportResult.Exception;

                var response = await _blobService.UploadUserAccountingFileAsync(job.UserId, job.Id,
                    $"Comptabilité_du_{request.From:dd-MM-yyyy}_au_{request.To:dd-MM-yyyy}.{deliveriesExportResult.Data.Extension}", deliveriesExportResult.Data.Data, token);
                if (!response.Succeeded)
                    throw response.Exception;

                job.SetDownloadUrl(response.Data);
                job.CompleteJob(new AccountingExportSucceededEvent(job.Id));

                await _context.SaveChangesAsync(token);
                return Success();
            }
            catch (Exception e)
            {
                job.FailJob(e.Message, new AccountingExportFailedEvent(job.Id));
                await _context.SaveChangesAsync(token);
                
                return Failure("Une erreur est survenue pendant l'export de comptabilité.");
            }
        }
    }
}