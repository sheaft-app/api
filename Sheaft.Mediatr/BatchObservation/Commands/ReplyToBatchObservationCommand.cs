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

namespace Sheaft.Mediatr.BatchObservation.Commands
{
    public class ReplyToBatchObservationCommand : Command
    {
        protected ReplyToBatchObservationCommand()
        {
        }

        [JsonConstructor]
        public ReplyToBatchObservationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid BatchObservationId { get; set; }
        public string Comment { get; set; }
    }

    public class ReplyToBatchObservationCommandHandler : CommandsHandler,
        IRequestHandler<ReplyToBatchObservationCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly RoleOptions _roleOptions;

        public ReplyToBatchObservationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ICurrentUserService currentUserService,
            IOptionsSnapshot<RoleOptions> roleOptions,
            ILogger<ReplyToBatchObservationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _currentUserService = currentUserService;
            _roleOptions = roleOptions.Value;
        }

        public async Task<Result> Handle(ReplyToBatchObservationCommand request, CancellationToken token)
        {
            var batchObservation = await _context.Set<Domain.BatchObservation>().SingleOrDefaultAsync(b => b.Id == request.BatchObservationId, token);
            if (batchObservation == null)
                return Failure("L'observation à laquelle répondre est introuvable.");

            var currentUser = _currentUserService.GetCurrentUserInfo();
            if (!currentUser.Succeeded)
                return Failure(currentUser);

            if (!currentUser.Data.IsInRole(_roleOptions.Producer.Value) &&
                batchObservation.UserId != currentUser.Data.Id)
                return Failure("Vous n'êtes pas autorisé à accéder à cette ressource.");
            
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.RequestUser.Id, token);
            batchObservation.AddReply(request.Comment, user);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}