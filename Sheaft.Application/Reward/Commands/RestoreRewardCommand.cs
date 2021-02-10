using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Reward.Commands
{
    public class RestoreRewardCommand : Command
    {
        [JsonConstructor]
        public RestoreRewardCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RewardId { get; set; }
    }

    public class RestoreRewardCommandHandler : CommandsHandler,
        IRequestHandler<RestoreRewardCommand, Result>
    {
        public RestoreRewardCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<RestoreRewardCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(RestoreRewardCommand request, CancellationToken token)
        {
            var entity = await _context.Rewards.SingleOrDefaultAsync(r => r.Id == request.RewardId, token);

            _context.Restore(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}