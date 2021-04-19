using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Transactions;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.Mediatr.Transactions.Commands
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
            if (job.User.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            try
            {
                var startResult =
                    await _mediatr.Process(new StartJobCommand(request.RequestUser) {JobId = job.Id}, token);
                if (!startResult.Succeeded)
                    throw startResult.Exception;

                _mediatr.Post(new TransactionsExportProcessingEvent(job.Id));

                var transactionsQuery = _context.Set<Transaction>().Where(o =>
                    o.Author.Id == request.RequestUser.Id
                    && o.Status == TransactionStatus.Succeeded
                    && o.ExecutedOn.HasValue && o.ExecutedOn.Value >= request.From
                    && o.ExecutedOn.HasValue && o.ExecutedOn.Value <= request.To);

                var exporter = await _transactionsExportersFactory.GetExporterAsync(request.RequestUser, token);
                var transactionsExportResult =
                    await exporter.ExportAsync(request.RequestUser, request.From, request.To, transactionsQuery, token);

                if (!transactionsExportResult.Succeeded)
                    throw transactionsExportResult.Exception;

                var response = await _blobService.UploadUserTransactionsFileAsync(job.User.Id, job.Id,
                    $"Virements_{request.From:dd-MM-yyyy}_{request.To:dd-MM-yyyy}.{transactionsExportResult.Data.Extension}", transactionsExportResult.Data.Data, token);
                if (!response.Succeeded)
                    throw response.Exception;

                _mediatr.Post(new TransactionsExportSucceededEvent(job.Id));

                _logger.LogInformation($"Transactions for user {job.User.Id} successfully exported");
                return await _mediatr.Process(
                    new CompleteJobCommand(request.RequestUser) {JobId = job.Id, FileUrl = response.Data}, token);
            }
            catch (Exception e)
            {
                _mediatr.Post(new TransactionsExportFailedEvent(job.Id));
                return await _mediatr.Process(
                    new FailJobCommand(request.RequestUser) {JobId = job.Id, Reason = e.Message},
                    token);
            }
        }
    }
}