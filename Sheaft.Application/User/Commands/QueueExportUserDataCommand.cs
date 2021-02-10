using Newtonsoft.Json;
using Sheaft.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class QueueExportUserDataCommand : Command<Guid>
    {
        [JsonConstructor]
        public QueueExportUserDataCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class QueueExportUserDataCommandHandler : CommandsHandler,
        IRequestHandler<QueueExportUserDataCommand, Result<Guid>>
    {
        private readonly IBlobService _blobService;
        private readonly ScoringOptions _scoringOptions;
        private readonly RoleOptions _roleOptions;

        public QueueExportUserDataCommandHandler(
            IOptionsSnapshot<ScoringOptions> scoringOptions,
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IBlobService blobService,
            ILogger<QueueExportUserDataCommandHandler> logger,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(mediatr, context, logger)
        {
            _roleOptions = roleOptions.Value;
            _scoringOptions = scoringOptions.Value;
            _blobService = blobService;
        }
        public async Task<Result<Guid>> Handle(QueueExportUserDataCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var sender = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);

                var entity = new Job(Guid.NewGuid(), JobKind.ExportUserData, $"Export RGPD", sender);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new ExportUserDataCommand(request.RequestUser) { Id = entity.Id }); ;
                _logger.LogInformation($"User RGPD data export successfully initiated by {request.RequestUser.Id}");

                return Ok(entity.Id);
            });
        }
    }
}
