using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Commands
{
    public class SetDeliveryModesAvailabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetDeliveryModesAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public bool Available { get; set; }
    }
    
    public class SetDeliveryModesAvailabilityCommandHandler : CommandsHandler,
        IRequestHandler<SetDeliveryModesAvailabilityCommand, Result<bool>>
    {
        public SetDeliveryModesAvailabilityCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SetDeliveryModesAvailabilityCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(SetDeliveryModesAvailabilityCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    foreach (var id in request.Ids)
                    {
                        var result = await _mediatr.Process(new SetDeliveryModeAvailabilityCommand(request.RequestUser) { Id = id, Available = request.Available }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }
    }
}
