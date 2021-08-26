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
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Billing.Commands
{
    public class MarkTimeRangeDeliveriesAsBilledCommand : Command<IEnumerable<Guid>>
    {
        protected MarkTimeRangeDeliveriesAsBilledCommand()
        {
        }

        [JsonConstructor]
        public MarkTimeRangeDeliveriesAsBilledCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public IEnumerable<DeliveryKind> Kinds { get; set; }
    }

    public class MarkTimeRangeDeliveriesAsBilledCommandHandler : CommandsHandler,
        IRequestHandler<MarkTimeRangeDeliveriesAsBilledCommand, Result<IEnumerable<Guid>>>
    {
        public MarkTimeRangeDeliveriesAsBilledCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<MarkTimeRangeDeliveriesAsBilledCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(MarkTimeRangeDeliveriesAsBilledCommand request, CancellationToken token)
        {
            if (request.Kinds == null || !request.Kinds.Any())
                request.Kinds = new List<DeliveryKind> {DeliveryKind.ProducerToStore};
            
            var deliveries = await _context.Set<Domain.Delivery>().Where(o =>
                    o.ProducerId == request.RequestUser.Id
                    & !o.BilledOn.HasValue
                    && o.Status == DeliveryStatus.Delivered
                    && request.Kinds.Contains(o.Kind)
                    && o.DeliveredOn >= request.From
                    && o.DeliveredOn <= request.To)
                .ToListAsync(token);

            if (deliveries == null || !deliveries.Any())
                return Failure<IEnumerable<Guid>>("Aucune livraison à archiver.");

            foreach (var delivery in deliveries)
                delivery.SetAsBilled();

            await _context.SaveChangesAsync(token);
            return Success<IEnumerable<Guid>>(deliveries.Select(d => d.Id).ToList());
        }
    }
}