using Sheaft.Application.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Application.Commands;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Domain.Enums;
using Sheaft.Application.Events;

namespace Sheaft.Application.Handlers
{
    public class SponsorCommandsHandler : ResultsHandler,
        IRequestHandler<CreateSponsoringCommand, Result<bool>>
    {
        public SponsorCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<SponsorCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(CreateSponsoringCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.UserId, token);
                var sponsor = await _context.FindSingleAsync<User>(u => u.SponsorshipCode == request.Code, token);
                if (sponsor == null)
                    return NotFound<bool>(MessageKind.Register_User_SponsorCode_NotFound);

                await _context.AddAsync(new Sponsoring(sponsor, user), token);
                await _context.SaveChangesAsync(token);

                await _mediatr.Post(new CreateUserPointsCommand(request.RequestUser)
                {
                    CreatedOn = DateTimeOffset.UtcNow,
                    Kind = PointKind.Sponsoring,
                    UserId = sponsor.Id
                }, token);

                await _mediatr.Post(new UserSponsoredEvent(request.RequestUser)
                {
                    SponsorId = sponsor.Id,
                    SponsoredId = user.Id
                }, token);

                return Ok(true);
            });
        }
    }
}