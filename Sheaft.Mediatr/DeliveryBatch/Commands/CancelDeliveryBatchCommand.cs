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
    public class CancelDeliveryBatchCommand : Command
    {
        protected CancelDeliveryBatchCommand()
        {
        }
        
        [JsonConstructor]
        public CancelDeliveryBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }

    public class CancelDeliveryBatchCommandHandler : CommandsHandler,
        IRequestHandler<CancelDeliveryBatchCommand, Result>
    {
        public CancelDeliveryBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CancelDeliveryBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CancelDeliveryBatchCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.Id, token);
            if (deliveryBatch == null)
                return Failure("La tournée de livraison est introuvable.");

            deliveryBatch.CancelBatch(request.Reason);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}