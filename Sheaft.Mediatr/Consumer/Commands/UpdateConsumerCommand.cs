using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Mediatr.Auth.Commands;
using Sheaft.Mediatr.User.Commands;
using Sheaft.Options;

namespace Sheaft.Mediatr.Consumer.Commands
{
    public class UpdateConsumerCommand : Command
    {
        protected UpdateConsumerCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ConsumerId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public IEnumerable<PictureInputDto> Pictures { get; set; }
    }

    public class UpdateConsumerCommandHandler : CommandsHandler,
        IRequestHandler<UpdateConsumerCommand, Result>
    {
        private readonly IPictureService _imageService;
        private readonly RoleOptions _roleOptions;

        public UpdateConsumerCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPictureService imageService,
            ILogger<UpdateConsumerCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _imageService = imageService;
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(UpdateConsumerCommand request, CancellationToken token)
        {
            var entity = await _context.Consumers.SingleAsync(e => e.Id == request.ConsumerId, token);
            if(entity.Id != request.RequestUser.Id)
                return Failure("Vous n'êtes pas authorisé à accéder à cette ressource.");

            entity.SetEmail(request.Email);
            entity.SetPhone(request.Phone);
            entity.SetFirstname(request.FirstName);
            entity.SetLastname(request.LastName);
            
            entity.SetSummary(request.Summary);
            entity.SetDescription(request.Description);
            entity.SetFacebook(request.Facebook);
            entity.SetTwitter(request.Twitter);
            entity.SetWebsite(request.Website);
            entity.SetInstagram(request.Instagram);
            
            if (request.Pictures != null && request.Pictures.Any())
            {
                entity.ClearPictures();
                    
                var result = Success<string>();
                foreach (var picture in request.Pictures)
                {
                    var id = Guid.NewGuid();
                    result = await _imageService.HandleUserPictureAsync(entity, id, picture.Data, token);
                    if (!result.Succeeded)
                        break;

                    if (!string.IsNullOrWhiteSpace(result.Data))
                        entity.AddPicture(new ProfilePicture(id, result.Data, picture.Position));
                }

                if (!result.Succeeded)
                    return Failure(result);
            }
            
            await _context.SaveChangesAsync(token);

            var resultImage = await _mediatr.Process(new UpdateUserPreviewCommand(request.RequestUser)
            {
                UserId = entity.Id,
                Picture = request.Picture,
                SkipAuthUpdate = true
            }, token);

            if (!resultImage.Succeeded)
                return Failure(resultImage);

            var authResult = await _mediatr.Process(new UpdateAuthUserCommand(request.RequestUser)
            {
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Name = entity.Name,
                Phone = entity.Phone,
                Picture = entity.Picture,
                Roles = new List<Guid> {_roleOptions.Consumer.Id},
                UserId = entity.Id
            }, token);

            if (!authResult.Succeeded)
                return authResult;

            return Success();
        }
    }
}