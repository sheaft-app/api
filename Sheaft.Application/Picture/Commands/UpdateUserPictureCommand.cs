using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Auth.Commands;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Picture.Commands
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
            var entity = await _context.GetByIdAsync<Domain.User>(request.UserId, token);

            var resultImage =
                await _imageService.HandleUserPictureAsync(entity, request.Picture, request.OriginalPicture, token);
            if (!resultImage.Succeeded)
                return Failure<string>(resultImage.Exception);

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
                return Failure<string>(result.Exception);

            return Success(resultImage.Data);
        }
    }
}