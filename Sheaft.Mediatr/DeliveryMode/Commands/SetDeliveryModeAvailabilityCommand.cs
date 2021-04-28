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
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.DeliveryMode.Commands
{
    public class SetDeliveryModeAvailabilityCommand : Command
    {
        protected SetDeliveryModeAvailabilityCommand()
        {
            
        }
        [JsonConstructor]
        public SetDeliveryModeAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryModeId { get; set; }
        public bool Available { get; set; }
    }

    public class SetDeliveryModeAvailabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetDeliveryModeAvailabilityCommand, Result>
    {
        public SetDeliveryModeAvailabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetDeliveryModeAvailabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetDeliveryModeAvailabilityCommand request, CancellationToken token)
        {
            var entity = await _context.DeliveryModes.SingleAsync(e => e.Id == request.DeliveryModeId, token);
            if(entity.ProducerId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            entity.SetAvailability(request.Available);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}