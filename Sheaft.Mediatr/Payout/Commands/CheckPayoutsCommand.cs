using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Payout.Commands
{
    public class CheckPayoutsCommand : Command
    {
        [JsonConstructor]
        public CheckPayoutsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class CheckPayoutsCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayoutsCommand, Result>
    {
        public CheckPayoutsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CheckPayoutsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckPayoutsCommand request, CancellationToken token)
        {
            var skip = 0;
            const int take = 100;

            var payoutIds = await GetNextPayoutIdsAsync(skip, take, token);
            while (payoutIds.Any())
            {
                foreach (var payoutId in payoutIds)
                {
                    _mediatr.Post(new CheckPayoutCommand(request.RequestUser)
                    {
                        PayoutId = payoutId
                    });
                }

                skip += take;
                payoutIds = await GetNextPayoutIdsAsync(skip, take, token);
            }

            return Success();
        }

        private async Task<IEnumerable<Guid>> GetNextPayoutIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Payouts
                .Get(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created, true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}