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
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Tag>(request.TagId, token);

                var resultImage = await _imageService.HandleTagPictureAsync(entity, request.Picture, token);
                if (!resultImage.Success)
                    return Failed<string>(resultImage.Exception);

                entity.SetPicture(resultImage.Data);
                await _context.SaveChangesAsync(token);

                return Ok(resultImage.Data);
            });
        }
    }
}
