using Sheaft.Application.Interop;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Sheaft.Application.Handlers
{
    public class AuthCommandsHandler : ResultsHandler,
        IRequestHandler<UpdateAuthUserCommand, Result<bool>>,
        IRequestHandler<UpdateAuthUserPictureCommand, Result<bool>>,
        IRequestHandler<RemoveAuthUserCommand, Result<bool>>
    {
        private readonly IAuthService _authService;
        private readonly IDistributedCache _cache;

        public AuthCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IAuthService authService,
            IDistributedCache cache,
            ILogger<AuthCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _authService = authService;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(UpdateAuthUserCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var result = await _authService.UpdateUserAsync(new Models.IdentityUserInput(request.UserId, request.Email, request.Name, request.FirstName, request.LastName, request.Roles), token);
                if (!result.Success)
                    return result;

                await _cache.RemoveAsync(request.UserId.ToString("N"));
                return result;
            });
        }

        public async Task<Result<bool>> Handle(UpdateAuthUserPictureCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var result = await _authService.UpdateUserPictureAsync(new Models.IdentityPictureInput(request.UserId, request.Picture), token);
                if (!result.Success)
                    return result;

                await _cache.RemoveAsync(request.UserId.ToString("N"));
                return result;
            });
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