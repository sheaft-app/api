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
    public class RestoreRewardCommand : Command<bool>
    {
        [JsonConstructor]
        public RestoreRewardCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
    
    public class RestoreRewardCommandHandler : CommandsHandler,
        IRequestHandler<RestoreRewardCommand, Result<bool>>
    {
        public RestoreRewardCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreRewardCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(RestoreRewardCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.Rewards.SingleOrDefaultAsync(r => r.Id == request.Id, token);

                _context.Restore(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}
