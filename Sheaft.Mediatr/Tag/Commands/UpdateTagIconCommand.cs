using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Tag.Commands
{
    public class UpdateTagIconCommand : Command<string>
    {
        [JsonConstructor]
        public UpdateTagIconCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TagId { get; set; }
        public string Icon { get; set; }
    }

    public class UpdateTagIconCommandHandler : CommandsHandler,
        IRequestHandler<UpdateTagIconCommand, Result<string>>
    {
        private readonly IPictureService _imageService;

        public UpdateTagIconCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<UpdateTagIconCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result<string>> Handle(UpdateTagIconCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Tag>(request.TagId, token);

            var resultImage = await _imageService.HandleTagIconAsync(entity, request.Icon, token);
            if (!resultImage.Succeeded)
                return Failure<string>(resultImage.Exception);

            entity.SetIcon(resultImage.Data);
            await _context.SaveChangesAsync(token);

            return Success(resultImage.Data);
        }
    }
}