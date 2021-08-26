using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Payout.Commands
{
    public class CheckNewPayoutsCommand : Command
    {
        protected CheckNewPayoutsCommand()
        {
            
        }
        [JsonConstructor]
        public CheckNewPayoutsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }

    public class CheckNewPayoutsCommandHandler : CommandsHandler,
        IRequestHandler<CheckNewPayoutsCommand, Result>
    {
        public CheckNewPayoutsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CheckNewPayoutsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckNewPayoutsCommand request, CancellationToken token)
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

            return Success();
        }

        private async Task<IEnumerable<KeyValuePair<Guid, List<Guid>>>> GetNextNewPayoutIdsAsync(int skip, int take,
            CancellationToken token)
        {
            var producersTransfers = await _context.Transfers
                .Where(t => t.Status == TransactionStatus.Succeeded
                            && t.CreditedWallet.User.Legal.Validation == LegalValidation.Regular
                            && (t.Payout == null || t.Payout.Status == TransactionStatus.Failed)
                            && t.PurchaseOrder.Status == PurchaseOrderStatus.Delivered)
                .Select(t => new {ProducerId = t.CreditedWallet.UserId, TransferId = t.Id})
                .OrderBy(c => c.ProducerId)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);

            var groupedProducers = producersTransfers.GroupBy(t => t.ProducerId);
            return groupedProducers.Select(c =>
                new KeyValuePair<Guid, List<Guid>>(c.Key, c.Select(t => t.TransferId)?.ToList()));
        }
    }
}