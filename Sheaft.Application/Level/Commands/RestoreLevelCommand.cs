using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;

namespace Sheaft.Application.Commands
{
    public class RestoreLevelCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }

    public class RestoreLevelCommandHandler : CommandsHandler,
        IRequestHandler<RestoreLevelCommand, Result<bool>>
    {
        public RestoreLevelCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreLevelCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(RestoreLevelCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Levels.SingleOrDefaultAsync(r => r.Id == request.Id, token);
                _context.Restore(entity);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
