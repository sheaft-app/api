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
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.Mediatr.DeliveryBatch.Commands
{
    public class StartDeliveryBatchCommand : Command
    {
        protected StartDeliveryBatchCommand()
        {
        }

        [JsonConstructor]
        public StartDeliveryBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryBatchId { get; set; }
        public bool StartFirstDelivery { get; set; }
    }

    public class StartDeliveryBatchCommandHandler : CommandsHandler,
        IRequestHandler<StartDeliveryBatchCommand, Result>
    {
        public StartDeliveryBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<StartDeliveryBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(StartDeliveryBatchCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.DeliveryBatchId, token);
            if (deliveryBatch == null)
                return Failure("La tournée de livraison est introuvable.");
            
            deliveryBatch.StartBatch();
            if (request.StartFirstDelivery)
            {
                var delivery = deliveryBatch.Deliveries.OrderBy(d => d.Position).First();
                delivery.StartDelivery();
            }
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}