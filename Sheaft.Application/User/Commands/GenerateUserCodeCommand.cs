using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class GenerateUserCodeCommand : Command<string>
    {
        [JsonConstructor]
        public GenerateUserCodeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class GenerateUserCodeCommandHandler : CommandsHandler,
        IRequestHandler<GenerateUserCodeCommand, Result<string>>
    {
        private readonly IBlobService _blobService;
        private readonly IIdentifierService _identifierService;
        private readonly ScoringOptions _scoringOptions;
        private readonly RoleOptions _roleOptions;

        public GenerateUserCodeCommandHandler(
            IOptionsSnapshot<ScoringOptions> scoringOptions,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            IIdentifierService identifierService,
            ILogger<GenerateUserCodeCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _scoringOptions = scoringOptions.Value;
            _blobService = blobService;
            _identifierService = identifierService;
        }

        public async Task<Result<string>> Handle(GenerateUserCodeCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<User>(request.Id, token);

                if (!string.IsNullOrWhiteSpace(entity.SponsorshipCode))
                    return Ok(entity.SponsorshipCode);

                var result = await _identifierService.GetNextSponsoringCode(token);
                if (!result.Success)
                    return Failed<string>(result.Exception);

                entity.SetSponsoringCode(result.Data);

                await _context.SaveChangesAsync(token);
                return Created(entity.SponsorshipCode);
            });
        }
    }
}
