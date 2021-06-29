using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Agreement.Commands;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.DeliveryMode.Commands
{
    public class DeleteDeliveryModeCommand : Command
    {
        protected DeleteDeliveryModeCommand()
        {
        }

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
            var entity = await _context.DeliveryModes.SingleAsync(e => e.Id == request.DeliveryModeId, token);
            if (entity.ProducerId != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            var agreements =
                await _context.Agreements.Where(a => a.DeliveryModeId == entity.Id).ToListAsync(token);
            if (agreements.Any(a => a.Status == AgreementStatus.Accepted))
                return Failure(MessageKind.DeliveryMode_CannotRemove_With_Active_Agreements, entity.Name,
                    agreements.Count(a => a.Status == AgreementStatus.Accepted));

            var orderDeliveries = await _context.Set<OrderDelivery>()
                .Where(o => o.DeliveryModeId == entity.Id)
                .ToListAsync(token);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var agreement in agreements)
                {
                    result =
                        await _mediatr.Process(
                            new DeleteAgreementCommand(request.RequestUser) {AgreementId = agreement.Id}, token);

                    if (!result.Succeeded)
                        break;
                }

                if (result is {Succeeded: false})
                    return result;

                foreach (var orderDelivery in orderDeliveries.ToList())
                    _context.Remove(orderDelivery);

                _context.Remove(entity);
                
                var canDirectSell = await _context.DeliveryModes.AnyAsync(
                    c => !c.RemovedOn.HasValue && c.ProducerId == entity.ProducerId &&
                         (c.Kind == DeliveryKind.Collective || c.Kind == DeliveryKind.Farm ||
                          c.Kind == DeliveryKind.Market), token);

                entity.Producer.SetCanDirectSell(canDirectSell);
                
                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);
            }
            
            return Success();
        }
    }
}