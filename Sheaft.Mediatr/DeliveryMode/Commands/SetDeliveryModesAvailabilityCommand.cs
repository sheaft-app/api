using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.DeliveryMode.Commands
{
    public class SetDeliveryModesAvailabilityCommand : Command
    {
        protected SetDeliveryModesAvailabilityCommand()
        {
        }

        [JsonConstructor]
        public SetDeliveryModesAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> DeliveryModeIds { get; set; }
        public bool Available { get; set; }
    }

    public class SetDeliveryModesAvailabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetDeliveryModesAvailabilityCommand, Result>
    {
        public SetDeliveryModesAvailabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetDeliveryModesAvailabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(SetDeliveryModesAvailabilityCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var id in request.DeliveryModeIds)
                {
                    result = await _mediatr.Process(
                        new SetDeliveryModeAvailabilityCommand(request.RequestUser)
                            {DeliveryModeId = id, Available = request.Available}, token);

                    if (!result.Succeeded)
                        break;
                }

                if (result is {Succeeded: false})
                    return result;

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}