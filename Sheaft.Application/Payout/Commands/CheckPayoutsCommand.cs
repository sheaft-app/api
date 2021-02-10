using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Payout.Commands
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
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;
        private readonly IPspService _pspService;

        public CheckPayoutsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CheckPayoutsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
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