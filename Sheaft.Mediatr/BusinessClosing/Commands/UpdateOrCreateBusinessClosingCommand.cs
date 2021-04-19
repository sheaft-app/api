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

namespace Sheaft.Mediatr.BusinessClosing.Commands
{
    public class UpdateOrCreateBusinessClosingCommand : Command<Guid>
    {
        protected UpdateOrCreateBusinessClosingCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateOrCreateBusinessClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public ClosingInputDto Closing { get; set; }
    }

    public class UpdateOrCreateBusinessClosingCommandHandler : CommandsHandler,
        IRequestHandler<UpdateOrCreateBusinessClosingCommand, Result<Guid>>
    {
        public UpdateOrCreateBusinessClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateOrCreateBusinessClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(UpdateOrCreateBusinessClosingCommand request, CancellationToken token)
        {
            var entity = await _context.Businesses.SingleAsync(e => e.Id == request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
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