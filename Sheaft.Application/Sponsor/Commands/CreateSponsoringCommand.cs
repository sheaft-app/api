using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;

namespace Sheaft.Application.Commands
{
    public class CreateSponsoringCommand : Command<bool>
    {
        [JsonConstructor]
        public CreateSponsoringCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
    
    public class CreateSponsoringCommandHandler : CommandsHandler,
        IRequestHandler<CreateSponsoringCommand, Result<bool>>
    {
        public CreateSponsoringCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateSponsoringCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(CreateSponsoringCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.UserId, token);
                var sponsor = await _context.FindSingleAsync<User>(u => u.SponsorshipCode == request.Code, token);
                if (sponsor == null)
                    return NotFound<bool>(MessageKind.Register_User_SponsorCode_NotFound);

                await _context.AddAsync(new Sponsoring(sponsor, user), token);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new CreateUserPointsCommand(request.RequestUser)
                {
                    CreatedOn = DateTimeOffset.UtcNow,
                    Kind = PointKind.Sponsoring,
                    UserId = sponsor.Id
                });

                _mediatr.Post(new UserSponsoredEvent(request.RequestUser)
                {
                    SponsorId = sponsor.Id,
                    SponsoredId = user.Id
                });

                return Ok(true);
            });
        }
    }
}
