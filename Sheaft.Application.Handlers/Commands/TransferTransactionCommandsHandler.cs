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
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class TransferTransactionCommandsHandler : ResultsHandler,
        IRequestHandler<CreateTransferTransactionCommand, Result<Guid>>,
        IRequestHandler<SetTransferStatusCommand, Result<bool>>,
        IRequestHandler<CheckTransferTransactionsCommand, Result<bool>>,
        IRequestHandler<CheckWaitingTransferTransactionCommand, Result<bool>>,
        IRequestHandler<CheckCreatedTransferTransactionCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;
        private readonly IMediator _mediatr;
        private readonly IQueueService _queueService;

        public TransferTransactionCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            IMediator mediatr,
            IQueueService queueService,
            ILogger<TransferTransactionCommandsHandler> logger) : base(logger)
        {
            _queueService = queueService;
            _context = context;
            _pspService = pspService;
            _mediatr = mediatr;
        }

        public async Task<Result<Guid>> Handle(CreateTransferTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var checkResult = await _mediatr.Send(new EnsureProducerConfiguredCommand(request.RequestUser) { Id = request.ToUserId }, token);
                if (!checkResult.Success)
                    return Failed<Guid>(checkResult.Exception);

                var debitedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.FromUserId, token);
                var creditedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.ToUserId, token);
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.PurchaseOrderId, token);

                var transfer = new TransferTransaction(Guid.NewGuid(), debitedWallet, creditedWallet, purchaseOrder);

                await _context.AddAsync(transfer, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateTransferAsync(transfer, token);
                if (!result.Success)
                {
                    return Failed<Guid>(result.Exception);
                }

                transfer.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                transfer.SetIdentifier(result.Data.Identifier);
                transfer.SetStatus(result.Data.Status);

                _context.Update(transfer);

                await _context.SaveChangesAsync(token);
                return Ok(transfer.Id);
            });
        }

        public async Task<Result<bool>> Handle(SetTransferStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetSingleAsync<TransferTransaction>(c => c.Identifier == request.Identifier, token);
                var pspResult = await _pspService.GetTransferAsync(transaction.Identifier, token);
                if (!pspResult.Success)
                    return Failed<bool>(pspResult.Exception);

                transaction.SetStatus(pspResult.Data.Status);
                transaction.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                transaction.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(transaction);
                var success = await _context.SaveChangesAsync(token) > 0;

                switch (request.Kind)
                {
                    case PspEventKind.TRANSFER_NORMAL_FAILED:
                        await _queueService.ProcessEventAsync(TransferFailedEvent.QUEUE_NAME, new TransferFailedEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                    case PspEventKind.TRANSFER_NORMAL_SUCCEEDED:
                        await _queueService.ProcessEventAsync(TransferSucceededEvent.QUEUE_NAME, new TransferSucceededEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                }

                return Ok(success);
            });
        }

        public async Task<Result<bool>> Handle(SetRefundTransferStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetSingleAsync<RefundTransferTransaction>(c => c.Identifier == request.Identifier, token);
                var pspResult = await _pspService.GetRefundAsync(transaction.Identifier, token);
                if (!pspResult.Success)
                    return Failed<bool>(pspResult.Exception);

                transaction.SetStatus(pspResult.Data.Status);
                transaction.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                transaction.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(transaction);
                var success = await _context.SaveChangesAsync(token) > 0;

                switch (request.Kind)
                {
                    case PspEventKind.TRANSFER_REFUND_FAILED:
                        await _queueService.ProcessEventAsync(RefundTransferFailedEvent.QUEUE_NAME, new RefundTransferFailedEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                    case PspEventKind.TRANSFER_REFUND_SUCCEEDED:
                        await _queueService.ProcessEventAsync(RefundTransferSucceededEvent.QUEUE_NAME, new RefundTransferSucceededEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                }

                return Ok(success);
            });
        }

        public async Task<Result<bool>> Handle(CheckTransferTransactionsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-15);
                var transactions = await GetNextTransferTransactions(expiredDate, skip, take, token);

                while (transactions.Any())
                {
                    foreach (var transaction in transactions)
                    {
                        switch (transaction.Status)
                        {
                            case TransactionStatus.Waiting:
                                await _queueService.ProcessCommandAsync(
                                    CheckWaitingTransferTransactionCommand.QUEUE_NAME,
                                    new CheckWaitingTransferTransactionCommand(request.RequestUser)
                                    {
                                        TransactionId = transaction.Id
                                    }, token);
                                break;
                            case TransactionStatus.Created:
                                await _queueService.ProcessCommandAsync(
                                    CheckCreatedTransferTransactionCommand.QUEUE_NAME,
                                    new CheckCreatedTransferTransactionCommand(request.RequestUser)
                                    {
                                        TransactionId = transaction.Id
                                    }, token);
                                break;
                        }
                    }

                    skip += take;
                    transactions = await GetNextTransferTransactions(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckWaitingTransferTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetByIdAsync<TransferTransaction>(request.TransactionId, token);
                if (transaction.Status != TransactionStatus.Waiting)
                    return Ok(true);

                transaction.SetStatus(TransactionStatus.Expired);
                _context.Update(transaction);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(CheckCreatedTransferTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetByIdAsync<TransferTransaction>(request.TransactionId, token);
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
                    //transaction.SetStatus(TransactionStatus.Expired);

                _context.Update(transaction);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        private async Task<IEnumerable<TransferTransaction>> GetNextTransferTransactions(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.Transactions
                                .OfType<TransferTransaction>()
                                .Get(c => (c.Status == TransactionStatus.Waiting && c.CreatedOn < expiredDate)
                                    || (c.Status == TransactionStatus.Created && c.UpdatedOn.HasValue && c.UpdatedOn.Value < expiredDate), true)
                                .OrderBy(c => c.Id)
                                .Skip(skip)
                                .Take(take)
                                .ToListAsync(token);
        }
    }
}
