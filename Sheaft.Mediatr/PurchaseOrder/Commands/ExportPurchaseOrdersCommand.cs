using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Business;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class ExportPurchaseOrdersCommand : Command
    {
        [JsonConstructor]
        public ExportPurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
    }

    public class ExportPurchaseOrdersCommandHandler : CommandsHandler,
        IRequestHandler<ExportPurchaseOrdersCommand, Result>
    {
        private readonly IBlobService _blobService;
        private readonly IPurchaseOrdersExportersFactory _purchaseOrdersExportersFactory;

        public ExportPurchaseOrdersCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            IPurchaseOrdersExportersFactory purchaseOrdersExportersFactory,
            ILogger<ExportPurchaseOrdersCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
            _purchaseOrdersExportersFactory = purchaseOrdersExportersFactory;
        }

        public async Task<Result> Handle(ExportPurchaseOrdersCommand request, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Domain.Job>(request.JobId, token);
            if (job.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            try
            {
                var startResult =
                    await _mediatr.Process(new StartJobCommand(request.RequestUser) {JobId = job.Id}, token);
                if (!startResult.Succeeded)
                    throw startResult.Exception;

                _mediatr.Post(new PurchaseOrdersExportProcessingEvent(job.Id));

                var purchaseOrdersQuery = _context.Set<Domain.PurchaseOrder>().Where(o =>
                    o.Vendor.Id == request.RequestUser.Id
                    && o.CreatedOn >= request.From
                    && o.CreatedOn <= request.To);

                var exporter = await _purchaseOrdersExportersFactory.GetExporterAsync(request.RequestUser, token);
                var purchaseOrdersExportResult = await exporter.ExportAsync(request.RequestUser, request.From, request.To,
                    purchaseOrdersQuery, token);

                if (!purchaseOrdersExportResult.Succeeded)
                    throw purchaseOrdersExportResult.Exception;

                var response = await _blobService.UploadUserPurchaseOrdersFileAsync(job.User.Id, job.Id,
                    $"Commandes{request.From:dd-MM-yyyy}_{request.To:dd-MM-yyyy}.{purchaseOrdersExportResult.Data.Extension}", purchaseOrdersExportResult.Data.Data, token);
                if (!response.Succeeded)
                    throw response.Exception;

                _mediatr.Post(new PurchaseOrdersExportSucceededEvent(job.Id));

                _logger.LogInformation($"PurchaseOrders for user {job.User.Id} successfully exported");
                return await _mediatr.Process(
                    new CompleteJobCommand(request.RequestUser) {JobId = job.Id, FileUrl = response.Data}, token);
            }
            catch (Exception e)
            {
                _mediatr.Post(new PurchaseOrdersExportFailedEvent(job.Id));
                return await _mediatr.Process(
                    new FailJobCommand(request.RequestUser) {JobId = job.Id, Reason = e.Message},
                    token);
            }
        }
    }
}