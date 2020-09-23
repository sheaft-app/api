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
    public class PayoutTransactionCommandsHandler : ResultsHandler,
        IRequestHandler<CreatePayoutTransactionCommand, Result<Guid>>,
        IRequestHandler<SetPayoutStatusCommand, Result<bool>>,
        IRequestHandler<CheckPayoutTransactionsCommand, Result<bool>>,
        IRequestHandler<CheckWaitingPayoutTransactionCommand, Result<bool>>,
        IRequestHandler<CheckCreatedPayoutTransactionCommand, Result<bool>>,
        IRequestHandler<CheckForNewPayoutsCommand, Result<bool>>,
        IRequestHandler<CreatePayoutForTransfersCommand, Result<bool>>
    {
        private readonly PspOptions _pspOptions;
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;
        private readonly IQueueService _queueService;
        private readonly IMediator _mediatr;

        public PayoutTransactionCommandsHandler(
            IMediator mediatr,
            IAppDbContext context,
            IPspService pspService,
            IQueueService queueService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<PayoutTransactionCommandsHandler> logger) : base(logger)
        {
            _mediatr = mediatr;
            _queueService = queueService;
            _context = context;
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreatePayoutTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var debitedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.FromUserId, token);
                var bankAccount = await _context.GetSingleAsync<BankAccount>(c => c.Id == request.BankAccountId && c.IsActive, token);

                var payout = new PayoutTransaction(Guid.NewGuid(), request.Amount, debitedWallet, bankAccount);

                await _context.AddAsync(payout, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreatePayoutAsync(payout, token);
                if (!result.Success)
                {
                    return Failed<Guid>(result.Exception);
                }

                payout.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                payout.SetStatus(result.Data.Status);
                payout.SetCreditedAmount(result.Data.Credited);

                _context.Update(payout);

                await _context.SaveChangesAsync(token);
                return Ok(payout.Id);
            });
        }

        public async Task<Result<bool>> Handle(SetPayoutStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetSingleAsync<PayoutTransaction>(c => c.Identifier == request.Identifier, token);
                var pspResult = await _pspService.GetPayoutAsync(transaction.Identifier, token);
                if (!pspResult.Success)
                    return Failed<bool>(pspResult.Exception);

                transaction.SetStatus(pspResult.Data.Status);
                transaction.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                transaction.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(transaction);
                var success = await _context.SaveChangesAsync(token) > 0;

                switch (request.Kind)
                {
                    case PspEventKind.PAYOUT_NORMAL_FAILED:
                        await _queueService.ProcessEventAsync(PayoutFailedEvent.QUEUE_NAME, new PayoutFailedEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                    case PspEventKind.PAYOUT_NORMAL_SUCCEEDED:
                        await _queueService.ProcessEventAsync(PayoutSucceededEvent.QUEUE_NAME, new PayoutSucceededEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                }

                return Ok(success);
            });
        }

        public async Task<Result<bool>> Handle(CheckPayoutTransactionsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-15);
                var transactions = await GetNextPayoutTransactions(expiredDate, skip, take, token);

                while (transactions.Any())
                {
                    foreach (var transaction in transactions)
                    {
                        switch (transaction.Status)
                        {
                            case TransactionStatus.Waiting:
                                await _queueService.ProcessCommandAsync(
                                    CheckWaitingPayoutTransactionCommand.QUEUE_NAME,
                                    new CheckWaitingPayoutTransactionCommand(request.RequestUser)
                                    {
                                        TransactionId = transaction.Id
                                    }, token);
                                break;
                            case TransactionStatus.Created:
                                await _queueService.ProcessCommandAsync(
                                    CheckCreatedPayoutTransactionCommand.QUEUE_NAME,
                                    new CheckCreatedPayoutTransactionCommand(request.RequestUser)
                                    {
                                        TransactionId = transaction.Id
                                    }, token);
                                break;
                        }
                    }

                    skip += take;
                    transactions = await GetNextPayoutTransactions(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckWaitingPayoutTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetByIdAsync<PayoutTransaction>(request.TransactionId, token);
                if (transaction.Status != TransactionStatus.Waiting)
                    return Ok(true);

                transaction.SetStatus(TransactionStatus.Expired);
                _context.Update(transaction);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(CheckCreatedPayoutTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetByIdAsync<PayoutTransaction>(request.TransactionId, token);
                if (transaction.Status != TransactionStatus.Created)
                    return Ok(true);

                var pspResult = await _pspService.GetTransferAsync(transaction.Identifier, token);
                if (!pspResult.Success)
                    return Failed<bool>(pspResult.Exception);

                transaction.SetStatus(pspResult.Data.Status);
                transaction.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                transaction.SetProcessedOn(pspResult.Data.ProcessedOn);

                //TODO validate this !
                //if (!transaction.ExecutedOn.HasValue)
                //    transaction.SetStatus(TransactionStatus.Expired);

                _context.Update(transaction);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckForNewPayoutsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var expiredDate = DateTimeOffset.UtcNow.AddDays(-7);

                var userIds = await _context.TransferTransactions
                    .Get(t => t.Payout == null
                            && t.Status == TransactionStatus.Succeeded
                            && t.PurchaseOrder.Status == PurchaseOrderStatus.Completed
                            && t.UpdatedOn.HasValue && t.UpdatedOn.Value < expiredDate)
                    .Select(t => t.CreditedWallet.User.Id)
                    .Distinct()
                    .ToListAsync(token);

                foreach (var userId in userIds)
                {
                    await _queueService.ProcessCommandAsync(CreatePayoutForTransfersCommand.QUEUE_NAME,
                        new CreatePayoutForTransfersCommand(request.RequestUser)
                        {
                            UserId = userId
                        }, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CreatePayoutForTransfersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var checkResult = await _mediatr.Send(new EnsureProducerConfiguredCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!checkResult.Success)
                {
                    await _queueService.ProcessEventAsync(ProducerNotConfiguredEvent.QUEUE_NAME,
                        new ProducerNotConfiguredEvent(request.RequestUser)
                        {
                            UserId = request.UserId
                        }, token);

                    return Failed<bool>(checkResult.Exception);
                }

                //CHECK KYC created
                var checkKycCreatedResult = await _mediatr.Send(new EnsureProducerDocumentsCreatedCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!checkKycCreatedResult.Success)
                {
                    await _queueService.ProcessEventAsync(ProducerDocumentsNotCreatedEvent.QUEUE_NAME,
                        new ProducerDocumentsNotCreatedEvent(request.RequestUser)
                        {
                            UserId = request.UserId
                        }, token);

                    return Failed<bool>(checkKycCreatedResult.Exception);
                }

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-10080);
                var transfers = await _context.TransferTransactions
                    .Get(t => t.Payout == null
                            && t.CreditedWallet.User.Id == request.UserId
                            && t.Status == TransactionStatus.Succeeded
                            && t.PurchaseOrder.Status == PurchaseOrderStatus.Completed
                            && t.UpdatedOn.HasValue && t.UpdatedOn.Value < expiredDate)
                    .ToListAsync(token);

                var amount = transfers.Sum(t => t.Credited);

                //CHECK KYC created
                var checkKycReviewedResult = await _mediatr.Send(new EnsureProducerDocumentsReviewedCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!checkKycReviewedResult.Success)
                {
                    await _queueService.ProcessEventAsync(ProducerDocumentsNotReviewedEvent.QUEUE_NAME,
                        new ProducerDocumentsNotReviewedEvent(request.RequestUser)
                        {
                            UserId = request.UserId
                        }, token);

                    return Failed<bool>(checkKycReviewedResult.Exception);
                }

                //CHECK KYC validated
                var checkKycValidatedResult = await _mediatr.Send(new EnsureProducerDocumentsValidatedCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!checkKycValidatedResult.Success)
                {
                    await _queueService.ProcessEventAsync(ProducerDocumentsNotValidatedEvent.QUEUE_NAME,
                        new ProducerDocumentsNotValidatedEvent(request.RequestUser)
                        {
                            UserId = request.UserId
                        }, token);

                    return Failed<bool>(checkKycValidatedResult.Exception);
                }

                var wallet = await _context.GetSingleAsync<Wallet>(w => w.User.Id == request.UserId && w.Kind == WalletKind.Payments, token);
                var bankAccount = await _context.GetSingleAsync<BankAccount>(b => b.User.Id == request.UserId, token);

                var fees = await _context.AnyAsync<PayoutTransaction>(p => p.DebitedWallet.User.Id == request.UserId
                    && (p.Status == TransactionStatus.Created || p.Status == TransactionStatus.Succeeded), token) ? 0m : _pspOptions.ProducerFees;

                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var payout = new PayoutTransaction(Guid.NewGuid(), amount, wallet, bankAccount, fees);
                    foreach (var transfer in transfers)
                    {
                        payout.AddTransfer(transfer);
                    }

                    await _context.AddAsync(payout);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreatePayoutAsync(payout, token);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(token);
                        return Failed<bool>(result.Exception);
                    }

                    payout.SetIdentifier(result.Data.Identifier);
                    payout.SetStatus(result.Data.Status);
                    payout.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    payout.SetProcessedOn(result.Data.ExecutedOn);

                    _context.Update(payout);
                    await _context.SaveChangesAsync(token);

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        private async Task<IEnumerable<PayoutTransaction>> GetNextPayoutTransactions(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.PayoutTransactions
                                .Get(c => (c.Status == TransactionStatus.Waiting && c.CreatedOn < expiredDate)
                                        || (c.Status == TransactionStatus.Created && c.UpdatedOn.HasValue && c.UpdatedOn.Value < expiredDate), true)
                                .OrderBy(c => c.CreatedOn)
                                .Skip(skip)
                                .Take(take)
                                .ToListAsync(token);
        }
    }
}
