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
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class TransferCommandsHandler : ResultsHandler,
        IRequestHandler<CheckTransfersCommand, Result<bool>>,
        IRequestHandler<CheckTransferCommand, Result<bool>>,
        IRequestHandler<ExpireTransferCommand, Result<bool>>,
        IRequestHandler<RefreshTransferStatusCommand, Result<bool>>,
        IRequestHandler<CheckNewTransfersCommand, Result<bool>>,
        IRequestHandler<CreatePurchaseOrderTransferCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public TransferCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<TransferCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<bool>> Handle(RefreshTransferStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetSingleAsync<Transfer>(c => c.Identifier == request.Identifier, token);
                var pspResult = await _pspService.GetTransferAsync(transaction.Identifier, token);
                if (!pspResult.Success)
                    return Failed<bool>(pspResult.Exception);

                transaction.SetStatus(pspResult.Data.Status);
                transaction.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                transaction.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(transaction);
                var success = await _context.SaveChangesAsync(token) > 0;

                switch (transaction.Status)
                {
                    case TransactionStatus.Failed:
                        await _mediatr.Post(new TransferFailedEvent(request.RequestUser) { TransferId = transaction.Id }, token);
                        break;
                    case TransactionStatus.Succeeded:
                        await _mediatr.Post(new TransferSucceededEvent(request.RequestUser) { TransferId = transaction.Id }, token);
                        break;
                }

                return Ok(success);
            });
        }

        public async Task<Result<bool>> Handle(CheckTransfersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-15);
                var transferIds = await GetNextTransferIdsAsync(expiredDate, skip, take, token);

                while (transferIds.Any())
                {
                    foreach (var transferId in transferIds)
                    {
                        await _mediatr.Post(new CheckTransferCommand(request.RequestUser)
                        {
                            TransferId = transferId
                        }, token);
                        break;
                    }

                    skip += take;
                    transferIds = await GetNextTransferIdsAsync(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckTransferCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transfer = await _context.GetByIdAsync<Transfer>(request.TransferId, token);
                if (transfer.Status == TransactionStatus.Succeeded
                    && transfer.Status == TransactionStatus.Failed)
                    return Ok(true);

                if (transfer.CreatedOn.AddMinutes(10080) > DateTimeOffset.UtcNow)
                    return await _mediatr.Process(new ExpireTransferCommand(request.RequestUser) { TransferId = request.TransferId }, token);

                return await _mediatr.Process(new RefreshTransferStatusCommand(request.RequestUser, transfer.Identifier), token);
            });
        }

        public async Task<Result<bool>> Handle(ExpireTransferCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transfer = await _context.GetByIdAsync<Transfer>(request.TransferId, token);
                transfer.SetStatus(TransactionStatus.Expired);
                _context.Update(transfer);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckNewTransfersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-15);
                var purchaseOrders = await GetNextPurchaseOrdersForNewTransferAsync(expiredDate, skip, take, token);

                while (purchaseOrders.Any())
                {
                    foreach (var purchaseOrder in purchaseOrders)
                    {
                        await _mediatr.Post(new CreatePurchaseOrderTransferCommand(request.RequestUser)
                        {
                            PurchaseOrderId = purchaseOrder.Id
                        }, token);
                    }

                    skip += take;
                    purchaseOrders = await GetNextPurchaseOrdersForNewTransferAsync(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<Guid>> Handle(CreatePurchaseOrderTransferCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.PurchaseOrderId, token);
                if (purchaseOrder.Status < PurchaseOrderStatus.Accepted || purchaseOrder.Status > PurchaseOrderStatus.Delivered)
                    return Failed<Guid>(new InvalidOperationException());

                var transactions = purchaseOrder.Transfers.ToList();
                if (transactions.Any(c => c.Status != TransactionStatus.Failed && c.Status != TransactionStatus.Expired))
                    return Failed<Guid>(new InvalidOperationException());

                var checkResult = await _mediatr.Process(new CheckProducerConfigurationCommand(request.RequestUser) { UserId = purchaseOrder.Vendor.Id }, token);
                if (!checkResult.Success)
                    return Failed<Guid>(checkResult.Exception);

                if (transactions.Count(c => c.Status == TransactionStatus.Failed) >= 3)
                {
                    await _mediatr.Post(new CreatePurchaseOrderTransferFailedEvent(request.RequestUser)
                    {
                        PurchaseOrderId = purchaseOrder.Id
                    }, token);

                    return TooManyRetries<Guid>();
                }

                var debitedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == purchaseOrder.Sender.Id, token);
                var creditedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == purchaseOrder.Vendor.Id, token);

                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var transfer = new Transfer(Guid.NewGuid(), debitedWallet, creditedWallet, purchaseOrder);

                    await _context.AddAsync(transfer, token);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreateTransferAsync(transfer, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    transfer.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    transfer.SetIdentifier(result.Data.Identifier);
                    transfer.SetStatus(result.Data.Status);

                    _context.Update(transfer);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(transfer.Id);
                }
            });
        }

        private async Task<IEnumerable<PurchaseOrder>> GetNextPurchaseOrdersForNewTransferAsync(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.PurchaseOrders
                .Get(c => c.Status > PurchaseOrderStatus.Waiting
                            && c.Status < PurchaseOrderStatus.Refused
                            && c.UpdatedOn.Value < expiredDate
                            && (!c.Transfers.Any()
                                || c.Transfers.All(t => t.Status == TransactionStatus.Failed || t.Status == TransactionStatus.Expired)
                            ), true)
                .OrderBy(c => c.CreatedOn)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }

        private async Task<IEnumerable<Guid>> GetNextTransferIdsAsync(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.Transfers
                .Get(c => (c.Status == TransactionStatus.Waiting && c.CreatedOn < expiredDate)
                    || (c.Status == TransactionStatus.Created && c.UpdatedOn.HasValue && c.UpdatedOn.Value < expiredDate), true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
