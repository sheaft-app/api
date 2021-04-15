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
using Sheaft.Core.Exceptions;
using Sheaft.Domain;

namespace Sheaft.Mediatr.ProfileInformation.Commands
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
            
            entity.RemovePicture(request.PictureId);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}