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
    public class UpdateProductPictureCommand : Command<string>
    {
        [JsonConstructor]
        public UpdateProductPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public string Picture { get; set; }
        public string OriginalPicture { get; set; }
    }
    
    public class UpdateProductPictureCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProductPictureCommand, Result<string>>
    {
        private readonly IPictureService _imageService;

        public UpdateProductPictureCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<UpdateProductPictureCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result<string>> Handle(UpdateProductPictureCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Product>(request.ProductId, token);

                var resultImage = await _imageService.HandleProductPictureAsync(entity, request.Picture, request.OriginalPicture, token);
                if (!resultImage.Success)
                    return Failed<string>(resultImage.Exception);

                entity.SetPicture(resultImage.Data);
                await _context.SaveChangesAsync(token);

                return Ok(resultImage.Data);
            });
        }
    }
}
