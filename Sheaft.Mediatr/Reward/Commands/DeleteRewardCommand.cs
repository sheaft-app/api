using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Reward.Commands
{
    public class DeleteRewardCommand : Command
    {
        [JsonConstructor]
        public DeleteRewardCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RewardId { get; set; }
    }

    public class DeleteRewardCommandHandler : CommandsHandler,
        IRequestHandler<DeleteRewardCommand, Result>
    {
        public DeleteRewardCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<DeleteRewardCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(DeleteRewardCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Reward>(request.RewardId, token);

            _context.Remove(entity);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}