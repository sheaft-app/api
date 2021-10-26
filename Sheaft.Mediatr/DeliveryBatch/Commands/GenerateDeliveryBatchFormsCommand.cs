using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.DeliveryBatch.Commands
{
    public class GenerateDeliveryBatchFormsCommand : Command
    {
        protected GenerateDeliveryBatchFormsCommand()
        {
        }

        [JsonConstructor]
        public GenerateDeliveryBatchFormsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryBatchId { get; set; }
    }

    public class GenerateDeliveryBatchFormsCommandHandler : CommandsHandler,
        IRequestHandler<GenerateDeliveryBatchFormsCommand, Result>
    {
        private readonly IBlobService _blobService;

        public GenerateDeliveryBatchFormsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<GenerateDeliveryBatchFormsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _blobService = blobService;
        }

        public async Task<Result> Handle(GenerateDeliveryBatchFormsCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(d => d.Id == request.DeliveryBatchId, token);
            if (deliveryBatch == null)
                return Result.Failure($"DeliveryBatch {request.DeliveryBatchId} was not found.");
    
            var outputDocument = new PdfDocument();
            Result<byte[]> blobResult = null;
            foreach (var delivery in deliveryBatch.Deliveries)
            {
                blobResult = await _blobService.DownloadDeliveryAsync(delivery.DeliveryFormUrl, token);
                if (!blobResult.Succeeded)
                    break;

                using (var stream = new MemoryStream(blobResult.Data))
                {
                    var inputDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
                    for (var idx = 0; idx < inputDocument.PageCount; idx++)
                    {
                        var page = inputDocument.Pages[idx];
                        outputDocument.AddPage(page);
                    }
                }
            }

            if (!blobResult.Succeeded)
                return Failure(blobResult);

            outputDocument.Info.Subject = $"{deliveryBatch.Name} du {deliveryBatch.ScheduledOn:yyyyMMdd}";

            using (var stream = new MemoryStream())
            {
                outputDocument.Save(stream);
                var result = await _blobService.UploadProducerDeliveryBatchAsync(deliveryBatch.AssignedToId,
                    deliveryBatch.Id, $"{deliveryBatch.Name}_{deliveryBatch.ScheduledOn:yyyyMMdd}.pdf", stream.ToArray(), token);

                if (!result.Succeeded)
                    return Failure(result);

                deliveryBatch.SetDeliveryFormsUrl(result.Data);
                await _context.SaveChangesAsync(token);
            }

            return Success();
        }
    }
}