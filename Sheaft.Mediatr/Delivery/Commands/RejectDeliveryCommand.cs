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
using Sheaft.Core.Enums;

namespace Sheaft.Mediatr.Delivery.Commands
{
    public class RejectDeliveryCommand : Command
    {
        protected RejectDeliveryCommand()
        {
        }

        [JsonConstructor]
        public RejectDeliveryCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
        public string Reason { get; set; }
    }

    public class RejectDeliveryCommandHandler : CommandsHandler,
        IRequestHandler<RejectDeliveryCommand, Result>
    {
        public RejectDeliveryCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RejectDeliveryCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RejectDeliveryCommand request, CancellationToken token)
        {
            var delivery = await _context.Deliveries
                .SingleOrDefaultAsync(c => c.Id == request.DeliveryId, token);
            
            if (delivery == null)
                return Failure("La livraison est introuvable.");

            delivery.RejectDelivery(request.Reason);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}