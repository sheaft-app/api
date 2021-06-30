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
    public class StartDeliveryCommand : Command
    {
        protected StartDeliveryCommand()
        {
        }

        [JsonConstructor]
        public StartDeliveryCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
    }

    public class StartDeliveryCommandHandler : CommandsHandler,
        IRequestHandler<StartDeliveryCommand, Result>
    {
        public StartDeliveryCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<StartDeliveryCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(StartDeliveryCommand request, CancellationToken token)
        {
            var delivery = await _context.Deliveries
                .SingleOrDefaultAsync(c => c.Id == request.DeliveryId, token);
            if (delivery == null)
                return Failure("La livraison est introuvable.");

            delivery.StartDelivery();
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}