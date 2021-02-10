using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Application.Legal.Commands;
using Sheaft.Application.Wallet.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Producer.Commands
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
            var producer = await _context.FindByIdAsync<Domain.Producer>(request.ProducerId, token);            
            producer.CanDirectSell = await _context.DeliveryModes.AnyAsync(
                c => !c.RemovedOn.HasValue && c.Producer.Id == producer.Id &&
                     (c.Kind == DeliveryKind.Collective || c.Kind == DeliveryKind.Farm ||
                      c.Kind == DeliveryKind.Market), token);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}