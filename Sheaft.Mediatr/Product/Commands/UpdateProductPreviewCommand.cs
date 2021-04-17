using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Product.Commands
{
    public class UpdateProductPreviewCommand : Command<string>
    {
        [JsonConstructor]
        public UpdateProductPreviewCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProductId { get; set; }
        public PictureSourceDto Picture { get; set; }
    }

    public class UpdateProductPreviewCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProductPreviewCommand, Result<string>>
    {
        private readonly IPictureService _imageService;

        public UpdateProductPreviewCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<UpdateProductPreviewCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result<string>> Handle(UpdateProductPreviewCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Product>(request.ProductId, token);
            if(entity.Producer.Id != request.RequestUser.Id)
                return Failure<string>(MessageKind.Forbidden);

            var resultImage =
                await _imageService.HandleProductPreviewAsync(entity, request.Picture.Resized, request.Picture.Original, token);
            if (!resultImage.Succeeded)
                return Failure<string>(resultImage);

            entity.SetPicture(resultImage.Data);
            await _context.SaveChangesAsync(token);

            return Success(resultImage.Data);
        }
    }
}