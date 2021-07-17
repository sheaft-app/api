using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Options;

namespace Sheaft.Mediatr.Observation.Commands
{
    public class ReplyToObservationCommand : Command
    {
        protected ReplyToObservationCommand()
        {
        }

        [JsonConstructor]
        public ReplyToObservationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ObservationId { get; set; }
        public string Comment { get; set; }
    }

    public class ReplyToObservationCommandHandler : CommandsHandler,
        IRequestHandler<ReplyToObservationCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly RoleOptions _roleOptions;

        public ReplyToObservationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ICurrentUserService currentUserService,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<ReplyToObservationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _currentUserService = currentUserService;
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(ReplyToObservationCommand request, CancellationToken token)
        {
            var observation = await _context.Observations.SingleOrDefaultAsync(b => b.Id == request.ObservationId, token);
            if (observation == null)
                return Failure("L'observation à laquelle répondre est introuvable.");

            var currentUser = _currentUserService.GetCurrentUserInfo();
            if (!currentUser.Succeeded)
                return Failure(currentUser);

            if (!currentUser.Data.IsInRole(_roleOptions.Producer.Value) &&
                observation.UserId != currentUser.Data.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.RequestUser.Id, token);
            observation.AddReply(request.Comment, user);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}