using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.ProfileInformation.Commands
{
    public class AddPictureToUserProfileCommand : Command
    {
        [JsonConstructor]
        public AddPictureToUserProfileCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid UserId { get; set; }
        public string Picture { get; set; }
    }

    public class AddPictureToUserProfileCommandHandler : CommandsHandler,
        IRequestHandler<AddPictureToUserProfileCommand, Result>
    {
        private readonly IPictureService _imageService;

        public AddPictureToUserProfileCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<AddPictureToUserProfileCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result> Handle(AddPictureToUserProfileCommand request, CancellationToken token)
        {
            var entity = await _context.FindByIdAsync<Domain.User>(request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            var id = Guid.NewGuid();
            var result = await _imageService.HandleProfilePictureAsync(entity, id, request.Picture, token);
            if (!result.Succeeded)
                return Failure(result.Exception);
            
            entity.ProfileInformation.AddPicture(new ProfilePicture(id, result.Data));
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}