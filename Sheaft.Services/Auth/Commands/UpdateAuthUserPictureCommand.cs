using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Services.Auth.Commands
{
    public class UpdateAuthUserPictureCommand : Command
    {
        [JsonConstructor]
        public UpdateAuthUserPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Picture { get; set; }
    }

    public class UpdateAuthUserPictureCommandHandler : CommandsHandler,
        IRequestHandler<UpdateAuthUserPictureCommand, Result>
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

        public async Task<Result> Handle(UpdateAuthUserPictureCommand request, CancellationToken token)
        {
            var result =
                await _authService.UpdateUserPictureAsync(
                    new IdentityPictureDto(request.UserId, request.Picture), token);
            if (!result.Succeeded)
                return result;

            await _cache.RemoveAsync(request.UserId.ToString("N"));
            return result;
        }
    }
}