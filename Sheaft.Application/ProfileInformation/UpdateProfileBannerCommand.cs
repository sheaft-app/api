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
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;

namespace Sheaft.Application.User.Commands
{
    public class UpdateProfileBannerCommand : Command
    {
        [JsonConstructor]
        public UpdateProfileBannerCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid UserId { get; set; }
        public PictureInput Picture { get; set; }
    }

    public class UpdateProfileBannerCommandHandler : CommandsHandler,
        IRequestHandler<UpdateProfileBannerCommand, Result>
    {
        private readonly IPictureService _imageService;

        public UpdateProfileBannerCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<UpdateProfileBannerCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result> Handle(UpdateProfileBannerCommand request, CancellationToken token)
        {
            var entity = await _context.FindByIdAsync<Domain.User>(request.UserId, token);
            if (entity.ProfileInformation == null)
                throw new ArgumentException(nameof(entity.ProfileInformation));
            
            var result = await _imageService.HandleProfileBannerAsync(entity, request.Picture.Resized, request.Picture.Original, token);
            if (!result.Succeeded)
                return Failure(result.Exception);
            
            entity.ProfileInformation.SetBanner(result.Data);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}