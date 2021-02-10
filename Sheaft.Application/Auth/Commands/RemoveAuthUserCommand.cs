using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Commands
{
    public class RemoveAuthUserCommand : Command<bool>
    {
        [JsonConstructor]
        public RemoveAuthUserCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }

    public class RemoveAuthUserCommandHandler : CommandsHandler,
        IRequestHandler<RemoveAuthUserCommand, Result<bool>>
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

        public async Task<Result<bool>> Handle(RemoveAuthUserCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var result = await _authService.RemoveUserAsync(request.UserId, token);
                if (!result.Success)
                    return result;

                await _cache.RemoveAsync(request.UserId.ToString("N"));
                return result;
            });
        }
    }
}
