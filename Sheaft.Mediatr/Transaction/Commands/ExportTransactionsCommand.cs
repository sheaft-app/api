using System;
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
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Transactions;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.Mediatr.Transaction.Commands
{
    public class ExportTransactionsCommand : Command
    {
        protected ExportTransactionsCommand()
        {
            
        }
        [JsonConstructor]
        public ExportTransactionsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
    }

    public class ExportTransactionsCommandHandler : CommandsHandler,
        IRequestHandler<ExportTransactionsCommand, Result>
    {
        private readonly IBlobService _blobService;
        private readonly ITransactionsExportersFactory _transactionsExportersFactory;

        public ExportTransactionsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ITransactionsExportersFactory transactionsExportersFactory,
            ILogger<ExportTransactionsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
            _transactionsExportersFactory = transactionsExportersFactory;
        }

        public async Task<Result> Handle(ExportTransactionsCommand request, CancellationToken token)
        {
            var job = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if (job.UserId != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            try
            {
                job.StartJob(new TransactionsExportProcessingEvent(job.Id));
                await _context.SaveChangesAsync(token);

                var transactionsQuery = _context.Set<Domain.Transaction>().Where(o =>
                    o.AuthorId == request.RequestUser.Id
                    && o.Status == TransactionStatus.Succeeded
                    && o.ExecutedOn.HasValue && o.ExecutedOn.Value >= request.From
                    && o.ExecutedOn.HasValue && o.ExecutedOn.Value <= request.To);

                var exporter = await _transactionsExportersFactory.GetExporterAsync(request.RequestUser, token);
                var transactionsExportResult =
                    await exporter.ExportAsync(request.RequestUser, request.From, request.To, transactionsQuery, token);

                if (!transactionsExportResult.Succeeded)
                    throw transactionsExportResult.Exception;

                var response = await _blobService.UploadUserTransactionsFileAsync(job.UserId, job.Id,
                    $"Virements_du_{request.From:dd-MM-yyyy}_au_{request.To:dd-MM-yyyy}.{transactionsExportResult.Data.Extension}", transactionsExportResult.Data.Data, token);
                if (!response.Succeeded)
                    throw response.Exception;
                
                job.SetDownloadUrl(response.Data);
                job.CompleteJob(new TransactionsExportSucceededEvent(job.Id));

                await _context.SaveChangesAsync(token);
                return Success();
            }
            catch (Exception e)
            {
                job.FailJob(e.Message, new TransactionsExportFailedEvent(job.Id));
                await _context.SaveChangesAsync(token);
                
                return Failure("Une erreur est survenue pendant l'export des transactions.");
            }
        }
    }
}