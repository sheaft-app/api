using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.ProfileInformation.Commands
{
    public class RemoveUserPicturesCommand : Command
    {
        protected RemoveUserPicturesCommand()
        {
            
        }
        [JsonConstructor]
        public RemoveUserPicturesCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = RequestUser.Id;
        }
        
        public Guid UserId { get; set; }
        public List<Guid> PictureIds { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = RequestUser.Id;
        }
    }

    public class RemoveUserPicturesCommandHandler : CommandsHandler,
        IRequestHandler<RemoveUserPicturesCommand, Result>
    {
        public RemoveUserPicturesCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RemoveUserPicturesCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RemoveUserPicturesCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                Result result = null;
                foreach (var pictureId in request.PictureIds)
                {
                    result =
                        await _mediatr.Process(
                            new RemoveUserPictureCommand(request.RequestUser)
                                {PictureId = pictureId, UserId = request.UserId}, token);

                    if (!result.Succeeded)
                        break;
                }

                if (result is {Succeeded: false})
                    return result;

                await transaction.CommitAsync(token);
            }

            return Success();
        }
    }
}