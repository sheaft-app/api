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
using Sheaft.Application.Producer.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

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
            if(entity.Producer.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            var activeAgreements =
                await _context.Agreements.CountAsync(a => a.Delivery.Id == entity.Id && !a.RemovedOn.HasValue, token);
            if (activeAgreements > 0)
                return Failure(MessageKind.DeliveryMode_CannotRemove_With_Active_Agreements, entity.Name,
                    activeAgreements);

            _context.Remove(entity);            
            await _context.SaveChangesAsync(token);

            _mediatr.Post(new UpdateProducerAvailabilityCommand(request.RequestUser) {ProducerId = entity.Producer.Id});
            return Success();
        }
    }
}