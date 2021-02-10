using Newtonsoft.Json;
using Sheaft.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Commands
{
    public class RestoreTagCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreTagCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class RestoreTagCommandHandler : CommandsHandler,
        IRequestHandler<RestoreTagCommand, Result<bool>>
    {
        public RestoreTagCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreTagCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(RestoreTagCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Tags.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                _context.Restore(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
