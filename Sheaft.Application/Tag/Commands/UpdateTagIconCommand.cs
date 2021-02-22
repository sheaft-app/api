using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Tag.Commands
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