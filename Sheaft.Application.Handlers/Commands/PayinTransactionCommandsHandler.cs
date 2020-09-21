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
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Sheaft.Application.Handlers
{
    public class PayinTransactionCommandsHandler : ResultsHandler,
        IRequestHandler<CreateWebPayInTransactionCommand, Result<Guid>>,
        IRequestHandler<SetPayinStatusCommand, Result<bool>>,
        IRequestHandler<CheckPayinTransactionsCommand, Result<bool>>,
        IRequestHandler<CheckCreatedPayinTransactionCommand, Result<bool>>,
        IRequestHandler<CheckWaitingPayinTransactionCommand, Result<bool>>,
        IRequestHandler<SetRefundPayinStatusCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;
        private readonly IQueueService _queueService;
        private readonly IMediator _mediatr;

        public PayinTransactionCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            IQueueService queueService,
            IMediator mediatr,
            ILogger<PayinTransactionCommandsHandler> logger) : base(logger)
        {
            _mediatr = mediatr;
            _queueService = queueService;
            _context = context;
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateWebPayInTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                var wallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.RequestUser.Id, token);

                var webPayin = new WebPayinTransaction(Guid.NewGuid(), wallet, order);

                await _context.AddAsync(webPayin, token);
                await _context.SaveChangesAsync(token);

                var legal = await _context.GetSingleAsync<Legal>(c => c.Owner.Id == request.RequestUser.Id, token);
                var result = await _pspService.CreateWebPayinAsync(webPayin, legal.Owner, token);
                if (!result.Success)
                {
                    return Failed<Guid>(result.Exception);
                }

                webPayin.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                webPayin.SetIdentifier(result.Data.Identifier);
                webPayin.SetRedirectUrl(result.Data.RedirectUrl);
                webPayin.SetStatus(result.Data.Status);
                webPayin.SetCreditedAmount(result.Data.Credited);

                _context.Update(webPayin);

                await _context.SaveChangesAsync(token);
                return Ok(webPayin.Id);
            });
        }
        public async Task<Result<bool>> Handle(SetPayinStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetSingleAsync<WebPayinTransaction>(c => c.Identifier == request.Identifier, token);
                var pspResult = await _pspService.GetPayinAsync(transaction.Identifier, token);
                if (!pspResult.Success)
                    return Failed<bool>(pspResult.Exception);

                transaction.SetStatus(pspResult.Data.Status);
                transaction.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                transaction.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(transaction);
                var success = await _context.SaveChangesAsync(token) > 0;

                switch (request.Kind)
                {
                    case PspEventKind.PAYIN_NORMAL_FAILED:
                        await _queueService.ProcessEventAsync(PayinFailedEvent.QUEUE_NAME, new PayinFailedEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                    case PspEventKind.PAYIN_NORMAL_SUCCEEDED:
                        return await ConfirmPayinTransactionAsync(transaction, request.RequestUser, token);
                }

                return Ok(success);
            });
        }

        public async Task<Result<bool>> Handle(SetRefundPayinStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetSingleAsync<RefundPayinTransaction>(c => c.Identifier == request.Identifier, token);
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
                    case PspEventKind.PAYIN_REFUND_FAILED:
                        await _queueService.ProcessEventAsync(RefundPayinFailedEvent.QUEUE_NAME, new RefundPayinFailedEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                    case PspEventKind.PAYIN_REFUND_SUCCEEDED:
                        await _queueService.ProcessEventAsync(RefundPayinSucceededEvent.QUEUE_NAME, new RefundPayinSucceededEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                }

                return Ok(success);
            });
        }

        public async Task<Result<bool>> Handle(CheckPayinTransactionsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-15);
                var transactions = await GetNextPayinTransactions(expiredDate, skip, take, token);

                while (transactions.Any())
                {
                    foreach (var transaction in transactions)
                    {
                        switch (transaction.Status)
                        {
                            case TransactionStatus.Waiting:
                                await _queueService.ProcessCommandAsync(
                                    CheckWaitingPayinTransactionCommand.QUEUE_NAME,
                                    new CheckWaitingPayinTransactionCommand(request.RequestUser)
                                    {
                                        TransactionId = transaction.Id
                                    }, token);
                                break;
                            case TransactionStatus.Created:
                                await _queueService.ProcessCommandAsync(
                                    CheckCreatedPayinTransactionCommand.QUEUE_NAME,
                                    new CheckCreatedPayinTransactionCommand(request.RequestUser)
                                    {
                                        TransactionId = transaction.Id
                                    }, token);
                                break;
                        }
                    }

                    skip += take;
                    transactions = await GetNextPayinTransactions(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckWaitingPayinTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetByIdAsync<PayinTransaction>(request.TransactionId, token);
                if (transaction.Status != TransactionStatus.Waiting)
                    return Ok(true);

                transaction.SetStatus(TransactionStatus.Expired);
                _context.Update(transaction);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(CheckCreatedPayinTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetByIdAsync<PayinTransaction>(request.TransactionId, token);
                if (transaction.Status != TransactionStatus.Created)
                    return Ok(true);

                var pspResult = await _pspService.GetPayinAsync(transaction.Identifier, token);
                if (!pspResult.Success)
                    return Failed<bool>(pspResult.Exception);

                transaction.SetStatus(pspResult.Data.Status);
                transaction.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                transaction.SetProcessedOn(pspResult.Data.ProcessedOn);

                if (!transaction.ExecutedOn.HasValue)
                    transaction.SetStatus(TransactionStatus.Expired);

                _context.Update(transaction);
                await _context.SaveChangesAsync(token);

                if (transaction.Status == TransactionStatus.Succeeded)
                    return await ConfirmPayinTransactionAsync(transaction, request.RequestUser, token);

                return Ok(true);
            });
        }

        private async Task<Result<bool>> ConfirmPayinTransactionAsync(PayinTransaction transaction, RequestUser requestUser, CancellationToken token)
        {
            var orderResult = await _mediatr.Send(new ConfirmOrderCommand(requestUser) { Id = transaction.Order.Id }, token);
            if (!orderResult.Success)
                return Failed<bool>(orderResult.Exception);

            await _queueService.ProcessEventAsync(PayinSucceededEvent.QUEUE_NAME, new PayinSucceededEvent(requestUser) { TransactionId = transaction.Id }, token);
            return Ok(true);
        }

        private async Task<IEnumerable<PayinTransaction>> GetNextPayinTransactions(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.PayinTransactions
                                .Get(c => (c.Status == TransactionStatus.Waiting && c.CreatedOn < expiredDate)
                                            || (c.Status == TransactionStatus.Created && c.UpdatedOn.HasValue && c.UpdatedOn.Value < expiredDate), true)
                                .OrderBy(c => c.Id)
                                .Skip(skip)
                                .Take(take)
                                .ToListAsync(token);
        }
    }
}
