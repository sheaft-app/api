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
    public class RemoveUserDataCommand : Command<string>
    {
        [JsonConstructor]
        public RemoveUserDataCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Reason { get; set; }
    }
    
    public class RemoveUserDataCommandHandler : CommandsHandler,
        IRequestHandler<RemoveUserDataCommand, Result<string>>
    {
        private readonly IBlobService _blobService;
        private readonly ScoringOptions _scoringOptions;
        private readonly RoleOptions _roleOptions;

        public RemoveUserDataCommandHandler(
            IOptionsSnapshot<ScoringOptions> scoringOptions,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<RemoveUserDataCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _scoringOptions = scoringOptions.Value;
            _blobService = blobService;
        }
        public async Task<Result<string>> Handle(RemoveUserDataCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Users.FirstOrDefaultAsync(c => c.Id == request.Id, token);
                if (entity == null)
                    return NotFound<string>();

                if (!entity.RemovedOn.HasValue)
                    return Ok(request.Reason);

                await _blobService.CleanUserStorageAsync(request.Id, token);

                var result = await _mediatr.Process(new RemoveAuthUserCommand(request.RequestUser)
                {
                    UserId = entity.Id
                }, token);

                if (!result.Success)
                    return Failed<string>(result.Exception);

                entity.Close();
                await _context.SaveChangesAsync(token);

                return Ok(request.Reason);
            });
        }
    }
}
