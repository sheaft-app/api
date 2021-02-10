using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.DeliveryMode.Commands
{
    public class SetDeliveryModesAvailabilityCommand : Command
    {
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
                foreach (var id in request.DeliveryModeIds)
                {
                    var result = await _mediatr.Process(
                        new SetDeliveryModeAvailabilityCommand(request.RequestUser)
                            {DeliveryModeId = id, Available = request.Available}, token);
                    if (!result.Succeeded)
                        return Failure(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success();
            }
        }
    }
}