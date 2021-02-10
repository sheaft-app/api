using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class SetDeliveryModeAvailabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetDeliveryModeAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool Available { get; set; }
    }
    
    public class SetDeliveryModeAvailabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetDeliveryModeAvailabilityCommand, Result<bool>>
    {
        public SetDeliveryModeAvailabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetDeliveryModeAvailabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(SetDeliveryModeAvailabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<DeliveryMode>(request.Id, token);
                entity.SetAvailability(request.Available);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
