using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Extensions;
using Sheaft.Mailer.Helpers;

namespace Sheaft.Mediatr.Delivery.Commands
{
    public class GenerateDeliveryFormCommand : Command
    {
        protected GenerateDeliveryFormCommand()
        {
        }

        [JsonConstructor]
        public GenerateDeliveryFormCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
    }

    public class GenerateDeliveryFormCommandHandler : CommandsHandler,
        IRequestHandler<GenerateDeliveryFormCommand, Result>
    {
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IBlobService _blobService;

        public GenerateDeliveryFormCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPdfGenerator pdfGenerator,
            IBlobService blobService,
            ILogger<GenerateDeliveryFormCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pdfGenerator = pdfGenerator;
            _blobService = blobService;
        }

        public async Task<Result> Handle(GenerateDeliveryFormCommand request, CancellationToken token)
        {
            var delivery = await _context.Deliveries
                .SingleAsync(d => d.Id == request.DeliveryId, token);
            
            var producer = await _context.Producers.SingleAsync(p => p.Id == delivery.ProducerId, token);
            var producerSiret =
                (await _context.Set<BusinessLegal>().FirstOrDefaultAsync(b => b.UserId == producer.Id, token)).Siret;
            var client = await _context.Users.SingleAsync(s => s.Id == delivery.ClientId, token);
            var clientSiret =
                (await _context.Set<BusinessLegal>().FirstOrDefaultAsync(b => b.UserId == client.Id, token)).Siret;

            var data = DeliveryModelHelpers.GetDeliveryFormModel(producer, client, delivery, producerSiret, clientSiret);
            var result = await _pdfGenerator.GeneratePdfAsync("Bon de livraison", "DeliveryForm", data, token);
            if (!result.Succeeded)
                return Failure(result);

            var resultUrl = await _blobService.UploadProducerDeliveryFormAsync(delivery.ProducerId, delivery.Id,
                $"{delivery.Reference.AsDeliveryIdentifier()}.pdf", result.Data, token);
            if (!resultUrl.Succeeded)
                return Failure(resultUrl);

            delivery.SetFormUrl(resultUrl.Data);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}