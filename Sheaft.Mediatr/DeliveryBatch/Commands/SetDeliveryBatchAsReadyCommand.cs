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
    public class SetDeliveryBatchAsReadyCommand : Command
    {
        protected SetDeliveryBatchAsReadyCommand()
        {
        }

        [JsonConstructor]
        public SetDeliveryBatchAsReadyCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class SetDeliveryBatchAsReadyCommandHandler : CommandsHandler,
        IRequestHandler<SetDeliveryBatchAsReadyCommand, Result>
    {
        public SetDeliveryBatchAsReadyCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetDeliveryBatchAsReadyCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetDeliveryBatchAsReadyCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.Id, token);
            if (deliveryBatch == null)
                return Failure("La tournée de livraison est introuvable.");

            deliveryBatch.SetBatchReady();

            var result = await _mediatr.Process(new GenerateDeliveryBatchDocumentsCommand(request.RequestUser){Id = deliveryBatch.Id}, token);
            return result is {Succeeded: false} ? Failure(result) : Success();
        }
    }
}