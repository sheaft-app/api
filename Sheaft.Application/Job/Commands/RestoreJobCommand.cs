using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RestoreJobCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    public class RestoreJobCommandHandler : CommandsHandler,
        IRequestHandler<RestoreJobCommand, Result<bool>>
    {
        public RestoreJobCommandHandler(
            ISheaftMediatr mediatr, 
            IAppDbContext context,
            ILogger<RestoreJobCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }
        
        public async Task<Result<bool>> Handle(RestoreJobCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Jobs.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                _context.Restore(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
