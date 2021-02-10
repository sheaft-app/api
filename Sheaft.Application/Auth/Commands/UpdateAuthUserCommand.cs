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
    public class UpdateAuthUserCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateAuthUserCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public IEnumerable<Guid> Roles { get; set; }
    }

    public class UpdateAuthUserCommandHandler : CommandsHandler,
        IRequestHandler<UpdateAuthUserCommand, Result<bool>>
    {
        private readonly IAuthService _authService;
        private readonly IDistributedCache _cache;

        public UpdateAuthUserCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IAuthService authService,
            IDistributedCache cache,
            ILogger<UpdateAuthUserCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _authService = authService;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(UpdateAuthUserCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var result = await _authService.UpdateUserAsync(
                    new Models.IdentityUserInput(request.UserId, request.Email, request.Name, request.FirstName,
                        request.LastName, request.Picture, request.Roles), token);
                if (!result.Success)
                    return result;

                await _cache.RemoveAsync(request.UserId.ToString("N"));
                return result;
            });
        }
    }
}
