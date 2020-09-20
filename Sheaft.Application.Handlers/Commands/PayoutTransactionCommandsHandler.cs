using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Infrastructure.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using Sheaft.Services.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Application.Events;
using System.Linq;
using System.Collections.Generic;
using Sheaft.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class PayoutTransactionCommandsHandler : ResultsHandler,
        IRequestHandler<CreatePayoutTransactionCommand, Result<Guid>>,
        IRequestHandler<SetPayoutStatusCommand, Result<bool>>,
        IRequestHandler<CheckPayoutTransactionsCommand, Result<bool>>,
        IRequestHandler<CheckWaitingPayoutTransactionCommand, Result<bool>>,
        IRequestHandler<CheckCreatedPayoutTransactionCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;
        private readonly IQueueService _queueService;

        public PayoutTransactionCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            IQueueService queueService,
            ILogger<PayoutTransactionCommandsHandler> logger) : base(logger)
        {
            _queueService = queueService;
            _context = context;
            _pspService = pspService;
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
                if (!transaction.ExecutedOn.HasValue)
                    transaction.SetStatus(TransactionStatus.Expired);

                _context.Update(transaction);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        private async Task<IEnumerable<PayoutTransaction>> GetNextPayoutTransactions(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.Transactions
                                .OfType<PayoutTransaction>()
                                .Get(c => (c.Status == TransactionStatus.Waiting && c.CreatedOn < expiredDate)
                                        || (c.Status == TransactionStatus.Created && c.UpdatedOn.HasValue && c.UpdatedOn.Value < expiredDate), true)
                                .OrderBy(c => c.Id)
                                .Skip(skip)
                                .Take(take)
                                .ToListAsync(token);
        }
    }
}
