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
    public class RemoveUserProfilePictureCommand : Command
    {
        [JsonConstructor]
        public RemoveUserProfilePictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid UserId { get; set; }
        public Guid PictureId { get; set; }
    }

    public class RemoveUserProfilePictureCommandHandler : CommandsHandler,
        IRequestHandler<RemoveUserProfilePictureCommand, Result>
    {
        public RemoveUserProfilePictureCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RemoveUserProfilePictureCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RemoveUserProfilePictureCommand request, CancellationToken token)
        {
            var entity = await _context.FindByIdAsync<Domain.User>(request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            entity.ProfileInformation.RemovePicture(request.PictureId);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}