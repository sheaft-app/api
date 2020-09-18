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

namespace Sheaft.Application.Handlers
{
    public class TransferTransactionCommandsHandler : ResultsHandler,
        IRequestHandler<CreateTransferTransactionCommand, Result<Guid>>,
        IRequestHandler<SetTransferStatusCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;
        private readonly IMediator _mediatr;

        public TransferTransactionCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            IMediator mediatr,
            ILogger<TransferTransactionCommandsHandler> logger) : base(logger)
        {
            _context = context;
            _pspService = pspService;
            _mediatr = mediatr;
        }
        public async Task<Result<Guid>> Handle(CreateTransferTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
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
                transfer.SetCreditedAmount(result.Data.Credited);

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
                        await _mediatr.Publish(new TransferFailedEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                    case PspEventKind.TRANSFER_NORMAL_SUCCEEDED:
                        await _mediatr.Publish(new TransferSucceededEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
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
                        await _mediatr.Publish(new RefundTransferFailedEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                    case PspEventKind.TRANSFER_REFUND_SUCCEEDED:
                        await _mediatr.Publish(new RefundTransferSucceededEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                }

                return Ok(success);
            });
        }
    }
}
