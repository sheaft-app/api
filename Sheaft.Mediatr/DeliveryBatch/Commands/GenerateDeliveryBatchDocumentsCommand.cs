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
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.Mediatr.DeliveryBatch.Commands
{
    public class GenerateDeliveryBatchDocumentsCommand : Command
    {
        protected GenerateDeliveryBatchDocumentsCommand()
        {
        }

        [JsonConstructor]
        public GenerateDeliveryBatchDocumentsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class GenerateDeliveryBatchDocumentsCommandHandler : CommandsHandler,
        IRequestHandler<GenerateDeliveryBatchDocumentsCommand, Result>
    {
        public GenerateDeliveryBatchDocumentsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<GenerateDeliveryBatchDocumentsCommand> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(GenerateDeliveryBatchDocumentsCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.Id, token);
            if (deliveryBatch == null)
                return Failure("La tournée de livraison est introuvable.");

            Result result = null;
            foreach (var delivery in deliveryBatch.Deliveries)
            {
                result = await _mediatr.Process(
                    new GenerateDeliveryFormCommand(request.RequestUser) {DeliveryId = delivery.Id}, token);
                
                if (!result.Succeeded)
                    break;
            }

            if (result is {Succeeded: false})
                return Failure(result);

            await _context.SaveChangesAsync(token);
            
            _mediatr.Post(new GenerateDeliveryBatchFormsCommand(request.RequestUser)
                {DeliveryBatchId = deliveryBatch.Id});

            foreach (var delivery in deliveryBatch.Deliveries)
                _mediatr.Post(new GenerateDeliveryReceiptCommand(request.RequestUser) {DeliveryId = delivery.Id});
            
            return Success();
        }
    }
}