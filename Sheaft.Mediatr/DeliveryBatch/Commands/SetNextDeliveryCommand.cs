using System;
using System.Linq;
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
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.DeliveryBatch.Commands
{
    public class SetNextDeliveryCommand : Command
    {
        protected SetNextDeliveryCommand()
        {
        }

        [JsonConstructor]
        public SetNextDeliveryCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryBatchId { get; set; }
        public Guid DeliveryId { get; set; }
        public bool StartDelivery { get; set; }
    }

    public class SetNextDeliveryCommandHandler : CommandsHandler,
        IRequestHandler<SetNextDeliveryCommand, Result>
    {
        public SetNextDeliveryCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetNextDeliveryCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetNextDeliveryCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.DeliveryBatchId, token);
            if (deliveryBatch == null)
                return Failure("La tournée de livraison est introuvable.");

            var nextDelivery = deliveryBatch.Deliveries.SingleOrDefault(d => d.Id == request.DeliveryId);
            if (nextDelivery == null)
                return Failure("La livraison est introuvable dans la tournée.");
            
            var undeliveredDeliveries = deliveryBatch.Deliveries.Where(d => d.Status != DeliveryStatus.Delivered)
                .OrderBy(d => d.Position);

            var currentDelivery = undeliveredDeliveries.First();
            if (nextDelivery.Id != currentDelivery.Id)
            {
                nextDelivery.SetPosition(currentDelivery.Position.Value);
                foreach (var delivery in undeliveredDeliveries)
                {
                    var position = delivery.Position.Value;
                    delivery.SetPosition(position++);
                }
            }

            if(request.StartDelivery)
                nextDelivery.StartDelivery();

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}