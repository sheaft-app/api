using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Business.Commands
{
    public class DeleteBusinessClosingCommand : Command
    {
        protected DeleteBusinessClosingCommand()
        {
            
        }
        [JsonConstructor]
        public DeleteBusinessClosingCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ClosingId { get; set; }
    }

    public class DeleteBusinessClosingCommandHandler : CommandsHandler,
        IRequestHandler<DeleteBusinessClosingCommand, Result>
    {
        public DeleteBusinessClosingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteBusinessClosingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteBusinessClosingCommand request, CancellationToken token)
        {
            var entity = await _context.Businesses.SingleAsync(c => c.Closings.Any(cc => cc.Id == request.ClosingId), token);
            if(entity.Id != request.RequestUser.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");

            entity.RemoveClosing(request.ClosingId);
            await _context.SaveChangesAsync(token);
            
            return Success();
        }
    }
}