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
    public class SkipDeliveryCommand : Command
    {
        protected SkipDeliveryCommand()
        {
        }

        [JsonConstructor]
        public SkipDeliveryCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
    }

    public class SkipDeliveryCommandHandler : CommandsHandler,
        IRequestHandler<SkipDeliveryCommand, Result>
    {
        public SkipDeliveryCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SkipDeliveryCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SkipDeliveryCommand request, CancellationToken token)
        {
            var delivery = await _context.Deliveries
                .SingleOrDefaultAsync(c => c.Id == request.DeliveryId, token);
            if (delivery == null)
                return Failure("La livraison est introuvable.");

            delivery.SkipDelivery();
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}