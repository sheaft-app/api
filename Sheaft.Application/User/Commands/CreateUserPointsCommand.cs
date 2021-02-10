using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.User;

namespace Sheaft.Application.User.Commands
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
        private readonly IBlobService _blobService;
        private readonly ScoringOptions _scoringOptions;
        private readonly RoleOptions _roleOptions;

        public CreateUserPointsCommandHandler(
            IOptionsSnapshot<ScoringOptions> scoringOptions,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<CreateUserPointsCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _scoringOptions = scoringOptions.Value;
            _blobService = blobService;
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