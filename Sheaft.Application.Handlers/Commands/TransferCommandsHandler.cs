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
using Sheaft.Options;
using Microsoft.Extensions.Options;

namespace Sheaft.Application.Handlers
{
    public class TransferCommandsHandler : ResultsHandler,
        IRequestHandler<CheckTransfersCommand, Result<bool>>,
        IRequestHandler<CheckTransferCommand, Result<bool>>,
        IRequestHandler<ExpireTransferCommand, Result<bool>>,
        IRequestHandler<UnblockTransferCommand, Result<bool>>,
        IRequestHandler<RefreshTransferStatusCommand, Result<TransactionStatus>>,
        IRequestHandler<CreateTransferCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public TransferCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<TransferCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckTransfersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-_routineOptions.CheckTransfersFromMinutes);
                var transferIds = await GetNextTransferIdsAsync(expiredDate, skip, take, token);

                while (transferIds.Any())
                {
                    foreach (var transferId in transferIds)
                    {
                        _mediatr.Post(new CheckTransferCommand(request.RequestUser)
                        {
                            TransferId = transferId
                        });
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
                if (transfer.Status != TransactionStatus.Created && transfer.Status != TransactionStatus.Waiting)
                    return Ok(false);

                if (transfer.CreatedOn.AddMinutes(_routineOptions.CheckTransferExpiredFromMinutes) < DateTimeOffset.UtcNow && transfer.Status == TransactionStatus.Waiting)
                    return await _mediatr.Process(new ExpireTransferCommand(request.RequestUser) { TransferId = request.TransferId }, token);

                var result = await _mediatr.Process(new RefreshTransferStatusCommand(request.RequestUser, transfer.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ExpireTransferCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transfer = await _context.GetByIdAsync<Transfer>(request.TransferId, token);
                transfer.SetStatus(TransactionStatus.Expired);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(UnblockTransferCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transfer = await _context.GetByIdAsync<Transfer>(request.TransferId, token);
                transfer.SetSkipBackgroundProcessing(false);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshTransferStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transfer = await _context.GetSingleAsync<Transfer>(c => c.Identifier == request.Identifier, token);
                if (transfer.Status == TransactionStatus.Succeeded || transfer.Status == TransactionStatus.Failed)
                    return Failed<TransactionStatus>(new InvalidOperationException());

                var pspResult = await _pspService.GetTransferAsync(transfer.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                transfer.SetStatus(pspResult.Data.Status);
                transfer.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                transfer.SetExecutedOn(pspResult.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);

                switch (transfer.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new TransferFailedEvent(request.RequestUser) { TransferId = transfer.Id });
                        break;
                    case TransactionStatus.Succeeded:
                        _mediatr.Post(new TransferSucceededEvent(request.RequestUser) { TransferId = transfer.Id });
                        break;
                }

                return Ok(transfer.Status);
            });
        }

        public async Task<Result<Guid>> Handle(CreateTransferCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.PurchaseOrderId, token);
                if ((purchaseOrder.Transfer != null && purchaseOrder.Transfer.Status != TransactionStatus.Expired)
                    || !purchaseOrder.AcceptedOn.HasValue 
                    || purchaseOrder.WithdrawnOn.HasValue)
                    return Failed<Guid>(new InvalidOperationException());

                var purchaseOrderTransfers = await _context.FindAsync<Transfer>(c => c.PurchaseOrder.Id == purchaseOrder.Id, token);
                if (purchaseOrderTransfers.Any(c => c.Status != TransactionStatus.Expired))
                    return Failed<Guid>(new InvalidOperationException());

                var checkResult = await _mediatr.Process(new CheckProducerConfigurationCommand(request.RequestUser) { ProducerId = purchaseOrder.Vendor.Id }, token);
                if (!checkResult.Success)
                    return Failed<Guid>(checkResult.Exception);

                if (purchaseOrderTransfers.Count(c => c.Status == TransactionStatus.Expired) >= 3)
                {
                    purchaseOrder.SetSkipBackgroundProcessing(true);
                    await _context.SaveChangesAsync(token);

                    _mediatr.Post(new CreateTransferFailedEvent(request.RequestUser)
                    {
                        PurchaseOrderId = purchaseOrder.Id
                    });

                    return TooManyRetries<Guid>();
                }

                var debitedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == purchaseOrder.Sender.Id, token);
                var creditedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == purchaseOrder.Vendor.Id, token);

                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var transfer = new Transfer(Guid.NewGuid(), debitedWallet, creditedWallet, purchaseOrder);
                    await _context.AddAsync(transfer, token);
                    await _context.SaveChangesAsync(token);

                    purchaseOrder.SetTransfer(transfer);

                    var result = await _pspService.CreateTransferAsync(transfer, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    transfer.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    transfer.SetIdentifier(result.Data.Identifier);
                    transfer.SetStatus(result.Data.Status);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(transfer.Id);
                }
            });
        }

        private async Task<IEnumerable<Guid>> GetNextTransferIdsAsync(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.Transfers
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
