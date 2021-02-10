using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class RestoreDeliveryModeCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    public class RestoreDeliveryModeCommandHandler : CommandsHandler,
        IRequestHandler<RestoreDeliveryModeCommand, Result<bool>>
    {
        public RestoreDeliveryModeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreDeliveryModeCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(RestoreDeliveryModeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.DeliveryModes.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);

                _context.Restore(entity);
                entity.Producer.CanDirectSell = await _context.DeliveryModes.AnyAsync(c => !c.RemovedOn.HasValue && c.Producer.Id == request.RequestUser.Id && (c.Kind == DeliveryKind.Collective || c.Kind == DeliveryKind.Farm || c.Kind == DeliveryKind.Market), token);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
