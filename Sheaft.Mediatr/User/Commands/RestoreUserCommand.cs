using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.User.Commands
{
    public class RestoreUserCommand : Command
    {
        protected RestoreUserCommand()
        {
            
        }
        [JsonConstructor]
        public RestoreUserCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }

    public class RestoreUserCommandHandler : CommandsHandler,
        IRequestHandler<RestoreUserCommand, Result>
    {
        public RestoreUserCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreUserCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreUserCommand request, CancellationToken token)
        {
            var entity = await _context.Users.SingleOrDefaultAsync(c => c.Id == request.UserId, token);
            if (entity == null)
                return Failure("Utilisateur introuvable.");

            if (!entity.RemovedOn.HasValue)
                return Success();
            
            if(entity.Id != request.RequestUser.Id)
                return Failure<string>("Vous n'êtes pas autorisé à accéder à cette ressource.");

            entity.Restore();
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}