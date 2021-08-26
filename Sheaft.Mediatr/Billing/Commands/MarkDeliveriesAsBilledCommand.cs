using System;
using System.Collections.Generic;
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

namespace Sheaft.Mediatr.Billing.Commands
{
    public class MarkDeliveriesAsBilledCommand : Command
    {
        protected MarkDeliveriesAsBilledCommand()
        {
        }

        [JsonConstructor]
        public MarkDeliveriesAsBilledCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> DeliveryIds { get; set; }
    }

    public class MarkDeliveriesAsBilledCommandHandler : CommandsHandler,
        IRequestHandler<MarkDeliveriesAsBilledCommand, Result>
    {
        public MarkDeliveriesAsBilledCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<MarkDeliveriesAsBilledCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(MarkDeliveriesAsBilledCommand request, CancellationToken token)
        {
            var deliveries = await _context.Deliveries
                .Where(c => request.DeliveryIds.Contains(c.Id))
                .ToListAsync(token);
            
            if (deliveries == null || !deliveries.Any())
                return Failure("Les livraisons sont introuvables.");

            foreach (var delivery in deliveries)
                delivery.SetAsBilled();
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}