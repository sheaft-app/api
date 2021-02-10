using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckPayoutsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckPayoutsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
    
    public class CheckPayoutsCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayoutsCommand, Result<bool>>
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

        public async Task<Result<bool>> Handle(CheckPayoutsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
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

                return Ok(true);
            });
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
