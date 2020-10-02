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

namespace Sheaft.Application.Handlers
{
    public class PayoutCommandsHandler : ResultsHandler,
        IRequestHandler<CheckPayoutsCommand, Result<bool>>,
        IRequestHandler<CheckPayoutCommand, Result<bool>>,
        IRequestHandler<ExpirePayoutCommand, Result<bool>>,
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

        public async Task<Result<bool>> Handle(CheckPayoutsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-_routineOptions.CheckPayoutsFromMinutes);
                var payoutIds = await GetNextPayoutIdsAsync(expiredDate, skip, take, token);

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
                    payoutIds = await GetNextPayoutIdsAsync(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckPayoutCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var payout = await _context.GetByIdAsync<Payout>(request.PayoutId, token);
                if (payout.Status != TransactionStatus.Created && payout.Status != TransactionStatus.Waiting)
                    return Ok(false);

                if (payout.CreatedOn.AddMinutes(_routineOptions.CheckPayoutExpiredFromMinutes) < DateTimeOffset.UtcNow && payout.Status == TransactionStatus.Waiting)
                    return await _mediatr.Process(new ExpirePayoutCommand(request.RequestUser) { PayoutId = request.PayoutId }, token);

                var result = await _mediatr.Process(new RefreshPayoutStatusCommand(request.RequestUser, payout.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ExpirePayoutCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetByIdAsync<Payout>(request.PayoutId, token);
                transaction.SetStatus(TransactionStatus.Expired);

                _context.Update(transaction);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayoutStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var payout = await _context.GetSingleAsync<Payout>(c => c.Identifier == request.Identifier, token);
                if (payout.Status == TransactionStatus.Succeeded || payout.Status == TransactionStatus.Failed)
                    return Failed<TransactionStatus>(new InvalidOperationException());

                var pspResult = await _pspService.GetPayoutAsync(payout.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                payout.SetStatus(pspResult.Data.Status);
                payout.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                payout.SetExecutedOn(pspResult.Data.ProcessedOn);

                _context.Update(payout);
                await _context.SaveChangesAsync(token);

                switch (payout.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new PayoutFailedEvent(request.RequestUser) { PayoutId = payout.Id });
                        break;
                    case TransactionStatus.Succeeded:
                        _mediatr.Post(new PayoutSucceededEvent(request.RequestUser) { PayoutId = payout.Id });
                        break;
                }

                return Ok(payout.Status);
            });
        }

        public async Task<Result<Guid>> Handle(CreatePayoutCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var checkConfigurationResult = await _mediatr.Process(new CheckProducerConfigurationCommand(request.RequestUser) { ProducerId = request.ProducerId }, token);
                if (!checkConfigurationResult.Success)
                    return Failed<Guid>(checkConfigurationResult.Exception);

                var wallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.ProducerId, token);
                var bankAccount = await _context.GetSingleAsync<BankAccount>(c => c.User.Id == request.ProducerId && c.IsActive, token);

                var transfers = await _context.GetAsync<Transfer>(t => request.TransferIds.Contains(t.Id)
                                                            && t.PurchaseOrder.Status == PurchaseOrderStatus.Delivered
                                                            && (t.Payout == null || t.Payout.Status == TransactionStatus.Expired), token);

                var hasAlreadyPaidComission = await _context.AnyAsync<Payout>(p => p.Fees > 0 && p.DebitedWallet.User.Id == request.ProducerId
                    && (p.Status == TransactionStatus.Waiting || p.Status == TransactionStatus.Created || p.Status == TransactionStatus.Succeeded), token);

                var amount = transfers.Sum(t => t.Credited);
                var fees = hasAlreadyPaidComission || amount < _pspOptions.ProducerFees ? 0m : _pspOptions.ProducerFees;
                if (!hasAlreadyPaidComission && fees == 0m)
                    return Failed<Guid>(new InvalidCastException());

                var checkDocumentsValidatedResult = await _mediatr.Process(new CheckProducerDocumentsValidatedCommand(request.RequestUser) { ProducerId = request.ProducerId }, token);
                if (!checkDocumentsValidatedResult.Success)
                    return Failed<Guid>(checkDocumentsValidatedResult.Exception);

                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var payout = new Payout(Guid.NewGuid(), amount, wallet, bankAccount, transfers, fees);
                    await _context.AddAsync(payout);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreatePayoutAsync(payout, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    payout.SetIdentifier(result.Data.Identifier);
                    payout.SetStatus(result.Data.Status);
                    payout.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    payout.SetExecutedOn(result.Data.ProcessedOn);

                    _context.Update(payout);
                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(payout.Id);
                }
            });
        }

        private async Task<IEnumerable<Guid>> GetNextPayoutIdsAsync(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.Payouts
                .Get(c => c.CreatedOn < expiredDate
                      && (c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created), true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
