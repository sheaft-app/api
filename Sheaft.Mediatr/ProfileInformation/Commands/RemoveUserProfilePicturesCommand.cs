using System;
using System.Collections.Generic;
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

namespace Sheaft.Mediatr.ProfileInformation.Commands
{
    public class RemoveUserProfilePicturesCommand : Command
    {
        [JsonConstructor]
        public RemoveUserProfilePicturesCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid UserId { get; set; }
        public List<Guid> PictureIds { get; set; }
    }

    public class RemoveUserProfilePicturesCommandHandler : CommandsHandler,
        IRequestHandler<RemoveUserProfilePicturesCommand, Result>
    {
        public RemoveUserProfilePicturesCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RemoveUserProfilePicturesCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RemoveUserProfilePicturesCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                foreach (var pictureId in request.PictureIds)
                {
                    var result =
                        await _mediatr.Process(
                            new RemoveUserProfilePictureCommand(request.RequestUser)
                                {PictureId = pictureId, UserId = request.UserId}, token);

                    if (!result.Succeeded)
                        return result;
                }

                await transaction.CommitAsync(token);
            }

            return Success();
        }
    }
}