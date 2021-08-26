using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Auth.Commands
{
    public class RemoveAuthUserCommand : Command
    {
        protected RemoveAuthUserCommand()
        {
            
        }
        [JsonConstructor]
        public RemoveAuthUserCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }

    public class RemoveAuthUserCommandHandler : CommandsHandler,
        IRequestHandler<RemoveAuthUserCommand, Result>
    {
        private readonly IAuthService _authService;
        private readonly IDistributedCache _cache;

        public RemoveAuthUserCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IAuthService authService,
            IDistributedCache cache,
            ILogger<RemoveAuthUserCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _authService = authService;
            _cache = cache;
        }

        public async Task<Result> Handle(RemoveAuthUserCommand request, CancellationToken token)
        {
            var result = await _authService.RemoveUserAsync(request.UserId, token);
            if (!result.Succeeded)
                return result;

            await _cache.RemoveAsync(request.UserId.ToString("N"));
            return result;
        }
    }
}