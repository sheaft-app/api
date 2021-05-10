using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;

namespace Sheaft.Mediatr.User.Commands
{
    public class RemoveUserPictureCommand : Command
    {
        protected RemoveUserPictureCommand()
        {
            
        }
        [JsonConstructor]
        public RemoveUserPictureCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid UserId { get; set; }
        public Guid PictureId { get; set; }
    }

    public class RemoveUserPictureCommandHandler : CommandsHandler,
        IRequestHandler<RemoveUserPictureCommand, Result>
    {
        public RemoveUserPictureCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RemoveUserPictureCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RemoveUserPictureCommand request, CancellationToken token)
        {
            var entity = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            entity.RemovePicture(request.PictureId);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}