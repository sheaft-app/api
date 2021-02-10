using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class RestoreUserCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreUserCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class RestoreUserCommandHandler : CommandsHandler,
        IRequestHandler<RestoreUserCommand, Result<bool>>
    {
        private readonly IBlobService _blobService;
        private readonly ScoringOptions _scoringOptions;
        private readonly RoleOptions _roleOptions;

        public RestoreUserCommandHandler(
            IOptionsSnapshot<ScoringOptions> scoringOptions,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<RestoreUserCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _scoringOptions = scoringOptions.Value;
            _blobService = blobService;
        }
        public async Task<Result<bool>> Handle(RestoreUserCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Users.FirstOrDefaultAsync(c => c.Id == request.Id, token);
                if (entity == null)
                    return NotFound<bool>();
                    
                if (!entity.RemovedOn.HasValue)
                    return BadRequest<bool>();

                entity.Restore();
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}
