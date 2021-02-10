using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.DeliveryMode.Commands
{
    public class RestoreDeliveryModeCommand : Command
    {
        [JsonConstructor]
        public RestoreDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryModeId { get; set; }
    }

    public class RestoreDeliveryModeCommandHandler : CommandsHandler,
        IRequestHandler<RestoreDeliveryModeCommand, Result>
    {
        public RestoreDeliveryModeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreDeliveryModeCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreDeliveryModeCommand request, CancellationToken token)
        {
            var entity =
                await _context.DeliveryModes.SingleOrDefaultAsync(a => a.Id == request.DeliveryModeId && a.RemovedOn.HasValue,
                    token);

            _context.Restore(entity);
            entity.Producer.CanDirectSell = await _context.DeliveryModes.AnyAsync(
                c => !c.RemovedOn.HasValue && c.Producer.Id == entity.Producer.Id &&
                     (c.Kind == DeliveryKind.Collective || c.Kind == DeliveryKind.Farm ||
                      c.Kind == DeliveryKind.Market), token);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}