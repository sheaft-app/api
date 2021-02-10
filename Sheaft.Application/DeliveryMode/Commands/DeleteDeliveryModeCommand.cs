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
    public class DeleteDeliveryModeCommand : Command
    {
        [JsonConstructor]
        public DeleteDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryModeId { get; set; }
    }

    public class DeleteDeliveryModeCommandHandler : CommandsHandler,
        IRequestHandler<DeleteDeliveryModeCommand, Result>
    {
        public DeleteDeliveryModeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteDeliveryModeCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteDeliveryModeCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.DeliveryMode>(request.DeliveryModeId, token);
            var activeAgreements =
                await _context.Agreements.CountAsync(a => a.Delivery.Id == entity.Id && !a.RemovedOn.HasValue, token);
            if (activeAgreements > 0)
                return Failure(MessageKind.DeliveryMode_CannotRemove_With_Active_Agreements, entity.Name,
                    activeAgreements);

            _context.Remove(entity);
            entity.Producer.CanDirectSell = await _context.DeliveryModes.AnyAsync(
                c => !c.RemovedOn.HasValue && c.Producer.Id == entity.Producer.Id &&
                     (c.Kind == DeliveryKind.Collective || c.Kind == DeliveryKind.Farm ||
                      c.Kind == DeliveryKind.Market), token);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}