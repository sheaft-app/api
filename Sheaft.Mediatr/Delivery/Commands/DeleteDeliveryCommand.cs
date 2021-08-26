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

namespace Sheaft.Mediatr.Delivery.Commands
{
    public class DeleteDeliveryCommand : Command
    {
        protected DeleteDeliveryCommand()
        {
        }
        
        [JsonConstructor]
        public DeleteDeliveryCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
    }

    public class DeleteDeliveryCommandHandler : CommandsHandler,
        IRequestHandler<DeleteDeliveryCommand, Result>
    {
        public DeleteDeliveryCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteDeliveryCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteDeliveryCommand request, CancellationToken token)
        {
            var delivery = await _context.Deliveries.SingleOrDefaultAsync(c => c.Id == request.DeliveryId, token);
            if (delivery == null)
                return Failure("La livraison est introuvable.");
            
            if(delivery.DeliveryBatchId.HasValue)
                delivery.DeliveryBatch.RemoveDelivery(delivery);
            
            _context.Remove(delivery);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}