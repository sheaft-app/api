using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.DeliveryClosing.Commands
{
    public class DeleteDeliveryClosingCommand : Command
    {
        [JsonConstructor]
        public DeleteDeliveryClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ClosingId { get; set; }
    }

    public class DeleteDeliveryClosingCommandHandler : CommandsHandler,
        IRequestHandler<DeleteDeliveryClosingCommand, Result>
    {
        public DeleteDeliveryClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteDeliveryClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteDeliveryClosingCommand request, CancellationToken token)
        {
            var entity = await _context.GetSingleAsync<Domain.DeliveryMode>(c => c.Closings.Any(cc => cc.Id == request.ClosingId), token);
            if(entity.Producer.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);

            entity.RemoveClosing(request.ClosingId);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}