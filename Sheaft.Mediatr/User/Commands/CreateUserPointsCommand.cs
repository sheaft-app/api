using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.User;
using Sheaft.Options;

namespace Sheaft.Mediatr.User.Commands
{
    public class CreateUserPointsCommand : Command
    {
        [JsonConstructor]
        public CreateUserPointsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public PointKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid UserId { get; set; }
    }

    public class CreateUserPointsCommandHandler : CommandsHandler,
        IRequestHandler<CreateUserPointsCommand, Result>
    {
        private readonly ScoringOptions _scoringOptions;

        public CreateUserPointsCommandHandler(
            IOptionsSnapshot<ScoringOptions> scoringOptions,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<CreateUserPointsCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _scoringOptions = scoringOptions.Value;
        }

        public async Task<Result> Handle(CreateUserPointsCommand request, CancellationToken token)
        {
            if (!_scoringOptions.Points.TryGetValue(request.Kind.ToString("G"), out int quantity))
                return Failure(MessageKind.Points_Scoring_Matching_ActionPoints_NotFound);

            var user = await _context.GetByIdAsync<Domain.User>(request.UserId, token);
            user.AddPoints(request.Kind, quantity, DateTimeOffset.UtcNow);

            await _context.SaveChangesAsync(token);

            _mediatr.Post(new UserPointsCreatedEvent(user.Id, request.Kind, quantity, request.CreatedOn));
            return Success();
        }
    }
}