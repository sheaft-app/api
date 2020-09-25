using Sheaft.Application.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Commands;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Domain.Enums;
using Sheaft.Application.Events;

namespace Sheaft.Application.Handlers
{

    public class AuthCommandsHandler : ResultsHandler,
        IRequestHandler<UpdateAuthUserCommand, Result<bool>>,
        IRequestHandler<UpdateAuthUserPictureCommand, Result<bool>>,
        IRequestHandler<RemoveAuthUserCommand, Result<bool>>
    {
        private readonly IAuthService _authService;

        public AuthCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IAuthService authService,
            ILogger<AuthCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _authService = authService;
        }

        public async Task<Result<bool>> Handle(UpdateAuthUserCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () => await _authService.UpdateUserAsync(new Models.IdentityUserInput(request.UserId, request.Email, request.Name, request.FirstName, request.LastName, request.Roles), token));
        }

        public async Task<Result<bool>> Handle(UpdateAuthUserPictureCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () => await _authService.UpdateUserPictureAsync(new Models.IdentityPictureInput(request.UserId, request.Picture), token));
        }

        public async Task<Result<bool>> Handle(RemoveAuthUserCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () => await _authService.RemoveUserAsync(request.UserId, token));
        }
    }
}