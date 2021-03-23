using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.User.Commands
{
    public class RestoreUserCommand : Command
    {
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
            var entity = await _context.Users.FirstOrDefaultAsync(c => c.Id == request.UserId, token);
            if (entity == null || !entity.RemovedOn.HasValue)
                return Failure();
            
            if(entity.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();

            entity.Restore();
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}