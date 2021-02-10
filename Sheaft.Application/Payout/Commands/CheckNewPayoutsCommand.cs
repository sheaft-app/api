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
    public class CheckNewPayoutsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckNewPayoutsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
    
    public class CheckNewPayoutsCommandHandler : CommandsHandler,
        IRequestHandler<CheckNewPayoutsCommand, Result<bool>>
    {
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;
        private readonly IPspService _pspService;

        public CheckNewPayoutsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CheckNewPayoutsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckNewPayoutsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var skip = 0;
                const int take = 100;

                var payoutIds = await GetNextNewPayoutIdsAsync(skip, take, token);
                while (payoutIds.Any())
                {
                    foreach (var payoutId in payoutIds)
                    {
                        _mediatr.Post(new CreatePayoutCommand(request.RequestUser)
                        {
                            ProducerId = payoutId.Key,
                            TransferIds = payoutId.Value
                        });
                    }

                    skip += take;
                    payoutIds = await GetNextNewPayoutIdsAsync(skip, take, token);
                }

                return Ok(true);
            });
        }

        private async Task<IEnumerable<KeyValuePair<Guid, List<Guid>>>> GetNextNewPayoutIdsAsync(int skip, int take, CancellationToken token)
        {
            var producersTransfers = await _context.Transfers
                .Get(t => t.Status == TransactionStatus.Succeeded
                        && t.CreditedWallet.User.Legal.Validation == LegalValidation.Regular
                        && (t.Payout == null || t.Payout.Status == TransactionStatus.Failed)
                        && t.PurchaseOrder.Status == PurchaseOrderStatus.Delivered)
                .Select(t => new { ProducerId = t.CreditedWallet.User.Id, TransferId = t.Id })
                .OrderBy(c => c.ProducerId)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);

            var groupedProducers = producersTransfers.GroupBy(t => t.ProducerId);
            return groupedProducers.Select(c => new KeyValuePair<Guid, List<Guid>>(c.Key, c.Select(t => t.TransferId)?.ToList()));
        }
    }
}
