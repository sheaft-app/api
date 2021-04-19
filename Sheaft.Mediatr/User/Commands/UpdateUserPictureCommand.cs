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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Auth.Commands;

namespace Sheaft.Mediatr.User.Commands
{
    public class UpdateUserPictureCommand : Command<string>
    {
        [JsonConstructor]
        public UpdateUserPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Picture { get; set; }
        public string OriginalPicture { get; set; }
        public bool SkipAuthUpdate { get; set; }
    }

    public class UpdateUserPictureCommandHandler : CommandsHandler,
        IRequestHandler<UpdateUserPictureCommand, Result<string>>
    {
        private readonly IPictureService _imageService;

        public UpdateUserPictureCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<UpdateUserPictureCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result<string>> Handle(UpdateUserPictureCommand request, CancellationToken token)
        {
            var entity = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                return Failure<string>(MessageKind.Forbidden);

            var resultImage =
                await _imageService.HandleUserPictureAsync(entity, request.Picture, request.OriginalPicture, token);
            if (!resultImage.Succeeded)
                return Failure<string>(resultImage);

            entity.SetPicture(resultImage.Data);
            await _context.SaveChangesAsync(token);

            if (request.SkipAuthUpdate)
                return Success(resultImage.Data);

            var result = await _mediatr.Process(new UpdateAuthUserPictureCommand(request.RequestUser)
            {
                Picture = entity.Picture,
                UserId = entity.Id
            }, token);

            if (!result.Succeeded)
                return Failure<string>(result);

            return Success(resultImage.Data);
        }
    }
}