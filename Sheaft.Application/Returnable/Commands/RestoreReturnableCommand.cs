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
    public class RestoreReturnableCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreReturnableCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class RestoreReturnableCommandHandler : CommandsHandler,
        IRequestHandler<RestoreReturnableCommand, Result<bool>>
    {
        public RestoreReturnableCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreReturnableCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(RestoreReturnableCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Returnables.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);     
                
                _context.Restore(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}
