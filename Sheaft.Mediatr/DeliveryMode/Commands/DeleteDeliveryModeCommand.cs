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
            if(entity.Producer.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            var agreements =
                await _context.Agreements.Where(a => a.Delivery.Id == entity.Id).ToListAsync(token);
            if (agreements.Any(a => a.Status == AgreementStatus.Accepted))
                return Failure(MessageKind.DeliveryMode_CannotRemove_With_Active_Agreements, entity.Name,
                    agreements.Count(a => a.Status == AgreementStatus.Accepted));

            var orderDeliveries = await _context.Set<OrderDelivery>()
                .Where(o => o.DeliveryMode.Id == entity.Id)
                .ToListAsync(token);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var agreement in agreements)
                {
                    var result =
                        await _mediatr.Process(
                            new DeleteAgreementCommand(request.RequestUser) {AgreementId = agreement.Id}, token);
                    if (!result.Succeeded)
                        return Failure(result);
                }
                
                foreach (var orderDelivery in orderDeliveries.ToList())
                    _context.Remove(orderDelivery);

                _context.Remove(entity);
                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);
            }

            _mediatr.Post(new UpdateProducerAvailabilityCommand(request.RequestUser) {ProducerId = entity.Producer.Id});
            return Success();
        }
    }
}
