using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Auth.Commands;

namespace Sheaft.Mediatr.User.Commands
{
    public class UpdateUserPreviewCommand : Command<string>
    {
        protected UpdateUserPreviewCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateUserPreviewCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Picture { get; set; }
        public bool SkipAuthUpdate { get; set; }
    }

    public class UpdateUserPreviewCommandHandler : CommandsHandler,
        IRequestHandler<UpdateUserPreviewCommand, Result<string>>
    {
        private readonly IPictureService _imageService;

        public UpdateUserPreviewCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<UpdateUserPreviewCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result<string>> Handle(UpdateUserPreviewCommand request, CancellationToken token)
        {
            var entity = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                return Failure<string>("Vous n'êtes pas autorisé à accéder à cette ressource.");

            var resultImage =
                await _imageService.HandleUserProfileAsync(entity, request.Picture, token);
            if (!resultImage.Succeeded)
                return Failure<string>(resultImage);
            
            entity.SetPicture(resultImage.Data);
            await _context.SaveChangesAsync(token);

            if (request.SkipAuthUpdate)
                return Success<string>(resultImage.Data);

            var result = await _mediatr.Process(new UpdateAuthUserPictureCommand(request.RequestUser)
            {
                Picture = entity.Picture,
                UserId = entity.Id
            }, token);

            if (!result.Succeeded)
                return Failure<string>(result);

            return Success<string>(resultImage.Data);
        }
    }
}