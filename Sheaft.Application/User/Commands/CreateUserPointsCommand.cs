using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CreateUserPointsCommand : Command<bool>
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
        IRequestHandler<CreateUserPointsCommand, Result<bool>>
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

        public async Task<Result<bool>> Handle(CreateUserPointsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                if (!_scoringOptions.Points.TryGetValue(request.Kind.ToString("G"), out int quantity))
                    return BadRequest<bool>(MessageKind.Points_Scoring_Matching_ActionPoints_NotFound);

                var user = await _context.GetByIdAsync<User>(request.UserId, token);
                user.AddPoints(request.Kind, quantity, DateTimeOffset.UtcNow);

                await _context.SaveChangesAsync(token);

                //_mediatr.Post(new UserPointsCreatedEvent(request.RequestUser) { UserId = user.Id, Kind = request.Kind, Points = quantity, CreatedOn = request.CreatedOn });
                return Ok(true);
            });
        }
    }
}
