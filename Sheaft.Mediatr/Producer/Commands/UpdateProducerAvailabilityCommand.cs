using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Producer.Commands
{
    public class UpdateProducerAvailabilityCommand : Command
    {
        [JsonConstructor]
        public UpdateProducerAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }

    public class UpdateProducerAvailabilityCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProducerAvailabilityCommand, Result>
    {
        public UpdateProducerAvailabilityCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<UpdateProducerAvailabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateProducerAvailabilityCommand request, CancellationToken token)
        {
            var producer = await _context.Producers.SingleAsync(e => e.Id == request.ProducerId, token);            
            producer.CanDirectSell = await _context.DeliveryModes.AnyAsync(
                c => !c.RemovedOn.HasValue && c.Producer.Id == producer.Id &&
                     (c.Kind == DeliveryKind.Collective || c.Kind == DeliveryKind.Farm ||
                      c.Kind == DeliveryKind.Market), token);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}