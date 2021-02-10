using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Commands
{
    public class UpdateAuthUserPictureCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateAuthUserPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Picture { get; set; }
    }

    public class UpdateAuthUserPictureCommandHandler : CommandsHandler,
        IRequestHandler<UpdateAuthUserPictureCommand, Result<bool>>
    {
        private readonly IAuthService _authService;
        private readonly IDistributedCache _cache;

        public UpdateAuthUserPictureCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IAuthService authService,
            IDistributedCache cache,
            ILogger<UpdateAuthUserPictureCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _authService = authService;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(UpdateAuthUserPictureCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var result =
                    await _authService.UpdateUserPictureAsync(
                        new Models.IdentityPictureInput(request.UserId, request.Picture), token);
                if (!result.Success)
                    return result;

                await _cache.RemoveAsync(request.UserId.ToString("N"));
                return result;
            });
        }
    }
}
