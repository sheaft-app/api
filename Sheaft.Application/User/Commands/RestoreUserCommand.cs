using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.User.Commands
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