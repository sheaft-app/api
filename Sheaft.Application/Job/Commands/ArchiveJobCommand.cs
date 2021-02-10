using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class ArchiveJobCommand : Command<bool>
    {
        [JsonConstructor]
        public ArchiveJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class ArchiveJobCommandHandler : CommandsHandler,
        IRequestHandler<ArchiveJobCommand, Result<bool>>
    {
        public ArchiveJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<ArchiveJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        public async Task<Result<bool>> Handle(ArchiveJobCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<Job>(request.Id, token);
                entity.ArchiveJob();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
