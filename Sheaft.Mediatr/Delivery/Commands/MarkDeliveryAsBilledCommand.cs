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
    public class MarkDeliveryAsBilledCommand : Command
    {
        protected MarkDeliveryAsBilledCommand()
        {
        }

        [JsonConstructor]
        public MarkDeliveryAsBilledCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
    }

    public class MarkDeliveryAsBilledCommandHandler : CommandsHandler,
        IRequestHandler<MarkDeliveryAsBilledCommand, Result>
    {
        public MarkDeliveryAsBilledCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<MarkDeliveryAsBilledCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(MarkDeliveryAsBilledCommand request, CancellationToken token)
        {
            var delivery = await _context.Deliveries
                .SingleOrDefaultAsync(c => c.Id == request.DeliveryId, token);
            
            if (delivery == null)
                return Failure("La livraison est introuvable.");

            delivery.SetAsBilled();
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}