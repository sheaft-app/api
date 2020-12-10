using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using Sheaft.Domain.Enums;
using Sheaft.Application.Events;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Sheaft.Exceptions;

namespace Sheaft.Application.Handlers
{
    public class PayoutCommandsHandler : ResultsHandler,
        IRequestHandler<CheckNewPayoutsCommand, Result<bool>>,
        IRequestHandler<CheckPayoutsCommand, Result<bool>>,
        IRequestHandler<CheckPayoutCommand, Result<bool>>,
        IRequestHandler<RefreshPayoutStatusCommand, Result<TransactionStatus>>,
        IRequestHandler<CreatePayoutCommand, Result<Guid>>
    {
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;
        private readonly IPspService _pspService;

        public PayoutCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<PayoutCommandsHandler> logger)
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

        public async Task<Result<bool>> Handle(CheckPayoutCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payout = await _context.GetByIdAsync<Payout>(request.PayoutId, token);
                if (payout.Status != TransactionStatus.Created && payout.Status != TransactionStatus.Waiting)
                    return Ok(false);

                var result = await _mediatr.Process(new RefreshPayoutStatusCommand(request.RequestUser, payout.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayoutStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payout = await _context.GetSingleAsync<Payout>(c => c.Identifier == request.Identifier, token);
                if (payout.Status == TransactionStatus.Succeeded || payout.Status == TransactionStatus.Failed)
                    return Ok(payout.Status);

                var pspResult = await _pspService.GetPayoutAsync(payout.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                payout.SetStatus(pspResult.Data.Status);
                payout.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                payout.SetExecutedOn(pspResult.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);

                switch (payout.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new PayoutFailedEvent(request.RequestUser) { PayoutId = payout.Id });
                        break;
                    case TransactionStatus.Succeeded:
                        if(payout.Withholdings.Any())
                            _mediatr.Post(new ProcessWithholdingsCommand(request.RequestUser) { PayoutId = payout.Id });
                        break;
                }

                return Ok(payout.Status);
            });
        }

        public async Task<Result<Guid>> Handle(CreatePayoutCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producerLegals = await _context.GetSingleAsync<BusinessLegal>(c => c.User.Id == request.ProducerId, token);           
                if (producerLegals.Validation != LegalValidation.Regular)
                    return Failed<Guid>(new BadRequestException(MessageKind.Payout_CannotCreate_User_NotValidated));

                var wallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.ProducerId, token);
                var bankAccount = await _context.GetSingleAsync<BankAccount>(c => c.User.Id == request.ProducerId && c.IsActive, token);

                var transfers = await _context.GetAsync<Transfer>(
                    t => request.TransferIds.Contains(t.Id)
                        && t.PurchaseOrder.Status == PurchaseOrderStatus.Delivered
                        && (t.Payout == null || t.Payout.Status == TransactionStatus.Failed), 
                    token);

                var pendingWithholdings = await _context.GetAsync<Withholding>(c => c.DebitedWallet.User.Id == request.ProducerId && c.Status == TransactionStatus.Waiting && (c.Payout == null || c.Payout.Status == TransactionStatus.Failed), token);
                var withholdings = new List<Withholding>();
                var amount = transfers.Sum(t => t.Credited);
                var holdingAmount = 0m;
                foreach(var withholding in pendingWithholdings.OrderBy(w => w.Debited))
                {
                    holdingAmount += withholding.Debited;
                    if (holdingAmount < amount)
                        withholdings.Add(withholding);
                    else break;
                }

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var payout = new Payout(Guid.NewGuid(), wallet, bankAccount, transfers, withholdings);
                    await _context.AddAsync(payout);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreatePayoutAsync(payout, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    payout.SetIdentifier(result.Data.Identifier);
                    payout.SetStatus(result.Data.Status);
                    payout.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    payout.SetExecutedOn(result.Data.ProcessedOn);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(payout.Id);
                }
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
