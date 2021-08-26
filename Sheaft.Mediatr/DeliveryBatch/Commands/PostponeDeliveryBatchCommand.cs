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
    public class PostponeDeliveryBatchCommand : Command
    {
        protected PostponeDeliveryBatchCommand()
        {
        }
        
        [JsonConstructor]
        public PostponeDeliveryBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public DateTimeOffset ScheduledOn { get; set; }
        public TimeSpan From { get; set; }
        public string Reason { get; set; }
    }

    public class PostponeDeliveryBatchCommandHandler : CommandsHandler,
        IRequestHandler<PostponeDeliveryBatchCommand, Result>
    {
        public PostponeDeliveryBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<PostponeDeliveryBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(PostponeDeliveryBatchCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.Id, token);
            if (deliveryBatch == null)
                return Failure("La tournée de livraison est introuvable.");

            deliveryBatch.PostponeBatch(request.ScheduledOn, request.From, request.Reason);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}