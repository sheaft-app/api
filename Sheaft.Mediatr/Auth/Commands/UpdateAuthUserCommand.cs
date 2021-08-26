using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Auth.Commands
{
    public class UpdateAuthUserCommand : Command
    {
        protected UpdateAuthUserCommand()
        {
            
        }
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
        IRequestHandler<UpdateAuthUserCommand, Result>
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

        public async Task<Result> Handle(UpdateAuthUserCommand request, CancellationToken token)
        {
            var result = await _authService.UpdateUserAsync(
                new IdentityUserDto(request.UserId, request.Email, request.Name, request.FirstName,
                    request.LastName, request.Picture, request.Roles), token);
            if (!result.Succeeded)
                return result;

            await _cache.RemoveAsync(request.UserId.ToString("N"));
            return result;
        }
    }
}