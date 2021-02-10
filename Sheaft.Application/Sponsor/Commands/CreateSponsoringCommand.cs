using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.User.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.User;

namespace Sheaft.Application.Sponsor.Commands
{
    public class CreateSponsoringCommand : Command
    {
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
            var user = await _context.GetByIdAsync<Domain.User>(request.UserId, token);
            var sponsor = await _context.FindSingleAsync<Domain.User>(u => u.SponsorshipCode == request.Code, token);
            if (sponsor == null)
                return Failure(MessageKind.Register_User_SponsorCode_NotFound);

            await _context.AddAsync(new Sponsoring(sponsor, user), token);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}