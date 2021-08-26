using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Extensions;
using Sheaft.Mailing.Helpers;

namespace Sheaft.Mediatr.Delivery.Commands
{
    public class GenerateDeliveryReceiptCommand : Command
    {
        protected GenerateDeliveryReceiptCommand()
        {
        }

        [JsonConstructor]
        public GenerateDeliveryReceiptCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
    }

    public class GenerateDeliveryReceiptCommandHandler : CommandsHandler,
        IRequestHandler<GenerateDeliveryReceiptCommand, Result>
    {
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IBlobService _blobService;

        public GenerateDeliveryReceiptCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPdfGenerator pdfGenerator,
            IBlobService blobService,
            ILogger<GenerateDeliveryReceiptCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pdfGenerator = pdfGenerator;
            _blobService = blobService;
        }

        public async Task<Result> Handle(GenerateDeliveryReceiptCommand request, CancellationToken token)
        {
            var delivery = await _context.Deliveries
                .Include(d => d.Products)
                .Include(d => d.PurchaseOrders)
                .Include(d => d.ReturnedReturnables)
                .SingleAsync(d => d.Id == request.DeliveryId, token);
            
            var producer = await _context.Producers.SingleAsync(p => p.Id == delivery.ProducerId, token);
            var client = await _context.Users.SingleAsync(s => s.Id == delivery.ClientId, token);
           
            var data = DeliveryModelHelpers.GetDeliveryFormModel(producer, client, delivery);
            var result = await _pdfGenerator.GeneratePdfAsync("Bon de réception", "DeliveryReceipt", data, token);
            if (!result.Succeeded)
                return Failure(result);
            
            var resultUrl = await _blobService.UploadProducerDeliveryReceiptAsync(delivery.ProducerId, delivery.Id, $"{delivery.Reference.AsReceiptIdentifier()}.pdf",  result.Data, token);
            if (!resultUrl.Succeeded)
                return Failure(resultUrl);
            
            delivery.SetReceiptUrl(resultUrl.Data);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}