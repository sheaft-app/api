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
using Sheaft.Domain.Models;
using Sheaft.Exceptions;

namespace Sheaft.Application.Commands
{
    public class DeleteDeliveryModeCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteDeliveryModeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class DeleteDeliveryModeCommandHandler : CommandsHandler,
        IRequestHandler<DeleteDeliveryModeCommand, Result<bool>>
    {
        public DeleteDeliveryModeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteDeliveryModeCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(DeleteDeliveryModeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<DeliveryMode>(request.Id, token);
                var activeAgreements = await _context.Agreements.CountAsync(a => a.Delivery.Id == entity.Id && !a.RemovedOn.HasValue, token);
                if (activeAgreements > 0)
                    return BadRequest<bool>(MessageKind.DeliveryMode_CannotRemove_With_Active_Agreements, entity.Name, activeAgreements);

                _context.Remove(entity);
                entity.Producer.CanDirectSell = await _context.DeliveryModes.AnyAsync(c => !c.RemovedOn.HasValue && c.Producer.Id == request.RequestUser.Id && (c.Kind == DeliveryKind.Collective || c.Kind == DeliveryKind.Farm || c.Kind == DeliveryKind.Market), token);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
