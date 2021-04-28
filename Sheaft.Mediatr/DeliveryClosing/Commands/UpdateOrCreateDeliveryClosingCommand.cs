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
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.DeliveryClosing.Commands
{
    public class UpdateOrCreateDeliveryClosingCommand : Command<Guid>
    {
        protected UpdateOrCreateDeliveryClosingCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateOrCreateDeliveryClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryId { get; set; }
        public ClosingInputDto Closing { get; set; }
    }

    public class UpdateOrCreateDeliveryClosingCommandHandler : CommandsHandler,
        IRequestHandler<UpdateOrCreateDeliveryClosingCommand, Result<Guid>>
    {
        public UpdateOrCreateDeliveryClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateOrCreateDeliveryClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(UpdateOrCreateDeliveryClosingCommand request, CancellationToken token)
        {
            var entity = await _context.DeliveryModes.SingleAsync(e => e.Id == request.DeliveryId, token);
            if(entity.ProducerId != request.RequestUser.Id)
                return Failure<Guid>(MessageKind.Forbidden);

            Guid closingId;
            if (request.Closing.Id.HasValue)
            {
                var closing = entity.Closings.SingleOrDefault(c => c.Id == request.Closing.Id);
                if (closing == null)
                return Failure<Guid>(MessageKind.NotFound);

                closing.ChangeClosedDates(request.Closing.From, request.Closing.To);
                closing.SetReason(request.Closing.Reason);
                closingId = closing.Id;
            }
            else
            {
                var result = entity.AddClosing(request.Closing.From, request.Closing.To, request.Closing.Reason);
                closingId = result.Id;
            }

            await _context.SaveChangesAsync(token);
            return Success(closingId);
        }
    }
}