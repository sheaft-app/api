using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.User.Commands
{
    public class GenerateUserCodeCommand : Command<string>
    {
        protected GenerateUserCodeCommand()
        {
        }
        
        [JsonConstructor]
        public GenerateUserCodeCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = requestUser.Id;
        }

        public Guid UserId { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            UserId = user.Id;
        }
    }

    public class GenerateUserCodeCommandHandler : CommandsHandler,
        IRequestHandler<GenerateUserCodeCommand, Result<string>>
    {
        private readonly IIdentifierService _identifierService;

        public GenerateUserCodeCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IIdentifierService identifierService,
            ILogger<GenerateUserCodeCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
        }

        public async Task<Result<string>> Handle(GenerateUserCodeCommand request, CancellationToken token)
        {
            var entity = await _context.Users.SingleAsync(e => e.Id == request.UserId, token);
            if(entity.Id != request.RequestUser.Id)
                return Failure<string>("Vous n'êtes pas autorisé à accéder à cette ressource.");

            if (!string.IsNullOrWhiteSpace(entity.SponsorshipCode))
                return Success<string>(entity.SponsorshipCode);

            var result = await _identifierService.GetNextSponsoringCode(token);
            if (!result.Succeeded)
                return Failure<string>(result);

            entity.SetSponsoringCode(result.Data);

            await _context.SaveChangesAsync(token);
            return Success<string>(entity.SponsorshipCode);
        }
    }
}