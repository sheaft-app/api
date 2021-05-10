using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.Mediatr.Sponsor.Commands
{
    public class CreateSponsoringCommand : Command
    {
        protected CreateSponsoringCommand()
        {
            
        }
        [JsonConstructor]
        public CreateSponsoringCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Code { get; set; }
    }

    public class CreateSponsoringCommandHandler : CommandsHandler,
        IRequestHandler<CreateSponsoringCommand, Result>
    {
        public CreateSponsoringCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateSponsoringCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CreateSponsoringCommand request, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            var sponsor = await _context.Users.SingleOrDefaultAsync(u => u.SponsorshipCode == request.Code, token);
            if (sponsor == null)
                return Failure(MessageKind.Register_User_SponsorCode_NotFound);

            await _context.AddAsync(new Sponsoring(sponsor, user), token);
            await _context.SaveChangesAsync(token);

            _mediatr.Post(new CreateUserPointsCommand(request.RequestUser)
            {
                Kind = PointKind.Sponsoring,
                UserId = sponsor.Id
            });
            
            return Success();
        }
    }
}