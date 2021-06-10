﻿using System;
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
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.Mediatr.PurchaseOrder.Commands
{
    public class ExportPurchaseOrdersCommand : Command
    {
        protected ExportPurchaseOrdersCommand()
        {
            
        }
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
            var job = await _context.Jobs.SingleAsync(e => e.Id == request.JobId, token);
            if (job.UserId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            try
            {
                job.StartJob(new PurchaseOrdersExportProcessingEvent(job.Id));
                await _context.SaveChangesAsync(token);

                var purchaseOrdersQuery = _context.Set<Domain.PurchaseOrder>().Where(o =>
                    o.ProducerId == request.RequestUser.Id
                    && o.CreatedOn >= request.From
                    && o.CreatedOn <= request.To);

                var exporter = await _purchaseOrdersExportersFactory.GetExporterAsync(request.RequestUser, token);
                var purchaseOrdersExportResult = await exporter.ExportAsync(request.RequestUser, request.From, request.To,
                    purchaseOrdersQuery, token);

                if (!purchaseOrdersExportResult.Succeeded)
                    throw purchaseOrdersExportResult.Exception;

                var response = await _blobService.UploadUserPurchaseOrdersFileAsync(job.UserId, job.Id,
                    $"Commandes{request.From:dd-MM-yyyy}_{request.To:dd-MM-yyyy}.{purchaseOrdersExportResult.Data.Extension}", purchaseOrdersExportResult.Data.Data, token);
                if (!response.Succeeded)
                    throw response.Exception;

                job.SetDownloadUrl(response.Data);
                job.CompleteJob(new PurchaseOrdersExportSucceededEvent(job.Id));

                await _context.SaveChangesAsync(token);
                return Success();
            }
            catch (Exception e)
            {
                job.FailJob(e.Message, new PurchaseOrdersExportFailedEvent(job.Id));
                await _context.SaveChangesAsync(token);
                
                return Failure(MessageKind.JobFailure);
            }
        }
    }
}