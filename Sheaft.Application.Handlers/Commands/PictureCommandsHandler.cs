using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Sheaft.Application.Handlers
{
    public class PictureCommandsHandler : ResultsHandler,
        IRequestHandler<UpdateUserPictureCommand, Result<string>>,
        IRequestHandler<UpdateProductPictureCommand, Result<string>>
    {
        private readonly IPictureService _imageService;

        public PictureCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<PictureCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result<string>> Handle(UpdateUserPictureCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.UserId, token);

                var resultImage = await _imageService.HandleUserPictureAsync(entity, request.Picture, token);
                if (!resultImage.Success)
                    return Failed<string>(resultImage.Exception);

                entity.SetPicture(resultImage.Data);

                _context.Update(entity);
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

        public async Task<Result<string>> Handle(UpdateProductPictureCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Product>(request.ProductId, token);

                var resultImage = await _imageService.HandleProductPictureAsync(entity, request.Picture, token);
                if (!resultImage.Success)
                    return Failed<string>(resultImage.Exception);

                entity.SetPicture(resultImage.Data);

                _context.Update(entity);
                await _context.SaveChangesAsync(token);

                return Ok(resultImage.Data);
            });
        }

        public async Task<Result<string>> Handle(UpdateTagPictureCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<Tag>(request.TagId, token);

                var resultImage = await _imageService.HandleTagPictureAsync(entity, request.Picture, token);
                if (!resultImage.Success)
                    return Failed<string>(resultImage.Exception);

                entity.SetPicture(resultImage.Data);

                _context.Update(entity);
                await _context.SaveChangesAsync(token);

                return Ok(resultImage.Data);
            });
        }
    }
}