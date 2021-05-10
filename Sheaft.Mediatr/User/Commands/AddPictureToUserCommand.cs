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
    public class AddPictureToUserCommand : Command
    {
        protected AddPictureToUserCommand()
        {
            
        }
        [JsonConstructor]
        public AddPictureToUserCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid UserId { get; set; }
        public string Picture { get; set; }
    }

    public class AddPictureToUserCommandHandler : CommandsHandler,
        IRequestHandler<AddPictureToUserCommand, Result>
    {
        private readonly IPictureService _imageService;

        public AddPictureToUserCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<AddPictureToUserCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
        }

        public async Task<Result> Handle(AddPictureToUserCommand request, CancellationToken token)
        {
            var entity = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                return Failure(MessageKind.Forbidden);
            
            var id = Guid.NewGuid();
            var result = await _imageService.HandleProfilePictureAsync(entity, id, request.Picture, token);
            if (!result.Succeeded)
                return Failure(result);
            
            entity.AddPicture(new ProfilePicture(id, result.Data));
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}