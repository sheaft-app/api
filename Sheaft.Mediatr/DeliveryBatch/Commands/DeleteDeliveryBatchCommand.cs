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

namespace Sheaft.Mediatr.DeliveryBatch.Commands
{
    public class DeleteDeliveryBatchCommand : Command
    {
        protected DeleteDeliveryBatchCommand()
        {
        }
        
        [JsonConstructor]
        public DeleteDeliveryBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryBatchId { get; set; }
    }

    public class DeleteDeliveryBatchCommandHandler : CommandsHandler,
        IRequestHandler<DeleteDeliveryBatchCommand, Result>
    {
        public DeleteDeliveryBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteDeliveryBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteDeliveryBatchCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.DeliveryBatchId, token);
            if (deliveryBatch == null)
                return Failure("La tournée de livraison est introuvable.");

            _context.Remove(deliveryBatch);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}