using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Tag.Commands
{
    public class UpdateTagPictureCommand : Command<string>
    {
        [JsonConstructor]
        public UpdateTagPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TagId { get; set; }
        public string Picture { get; set; }
    }

    public class UpdateTagPictureCommandHandler : CommandsHandler,
        IRequestHandler<UpdateTagPictureCommand, Result<string>>
    {
        private readonly IPictureService _imageService;

        public UpdateTagPictureCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<UpdateTagPictureCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result<string>> Handle(UpdateTagPictureCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Tag>(request.TagId, token);

            var resultImage = await _imageService.HandleTagPictureAsync(entity, request.Picture, token);
            if (!resultImage.Succeeded)
                return Failure<string>(resultImage.Exception);

            entity.SetPicture(resultImage.Data);
            await _context.SaveChangesAsync(token);

            return Success(resultImage.Data);
        }
    }
}