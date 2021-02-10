using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
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
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.UserId, token);

                var resultImage = await _imageService.HandleUserPictureAsync(entity, request.Picture, request.OriginalPicture, token);
                if (!resultImage.Success)
                    return Failed<string>(resultImage.Exception);

                entity.SetPicture(resultImage.Data);
                await _context.SaveChangesAsync(token);

                if (request.SkipAuthUpdate)
                    return Ok(resultImage.Data);

                var result = await _mediatr.Process(new UpdateAuthUserPictureCommand(request.RequestUser)
                {
                    Picture = entity.Picture,
                    UserId = entity.Id
                }, token);

                if (!result.Success)
                    return Failed<string>(result.Exception);

                return Ok(resultImage.Data);
            });
        }
    }
}
