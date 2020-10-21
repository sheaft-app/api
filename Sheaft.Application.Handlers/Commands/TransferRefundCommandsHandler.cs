using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Application.Events;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sheaft.Options;
using Microsoft.Extensions.Options;

namespace Sheaft.Application.Handlers
{
    public class TransferRefundCommandsHandler : ResultsHandler,
        IRequestHandler<CheckTransferRefundsCommand, Result<bool>>,
        IRequestHandler<CheckTransferRefundCommand, Result<bool>>,
        IRequestHandler<ExpireTransferRefundCommand, Result<bool>>,
        IRequestHandler<RefreshTransferRefundStatusCommand, Result<TransactionStatus>>,
        IRequestHandler<CreateTransferRefundCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public TransferRefundCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<TransferRefundCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateTransferRefundCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.PurchaseOrderId, token);
                if (purchaseOrder.Transfer == null 
                    || purchaseOrder.Transfer.Status != TransactionStatus.Succeeded
                    || (purchaseOrder.Transfer.Refund != null && purchaseOrder.Transfer.Refund.Status != TransactionStatus.Expired))
                    return Failed<Guid>(new InvalidOperationException());

                var purchaseOrderTransferRefunds = await _context.FindAsync<TransferRefund>(c => c.Transfer.Id == purchaseOrder.Transfer.Id, token);
                if (purchaseOrderTransferRefunds.Any(c => c.Status != TransactionStatus.Expired))
                    return Failed<Guid>(new InvalidOperationException());

                if (purchaseOrderTransferRefunds.Count(c => c.Status == TransactionStatus.Expired) >= 3)
                {
                    purchaseOrder.Transfer.SetSkipBackgroundProcessing(true);
                    await _context.SaveChangesAsync(token);

                    _mediatr.Post(new CreateTransferRefundFailedEvent(request.RequestUser)
                    {
                        PurchaseOrderId = purchaseOrder.Id
                    });

                    return TooManyRetries<Guid>();
                }

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var transferRefund = new TransferRefund(Guid.NewGuid(), purchaseOrder.Transfer);

                    await _context.AddAsync(transferRefund, token);
                    await _context.SaveChangesAsync(token);

                    purchaseOrder.Transfer.SetRefund(transferRefund);

                    var result = await _pspService.RefundTransferAsync(transferRefund, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);


                    transferRefund.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    transferRefund.SetIdentifier(result.Data.Identifier);
                    transferRefund.SetStatus(result.Data.Status);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(transferRefund.Id);
                }
            });
        }

        public async Task<Result<bool>> Handle(CheckTransferRefundsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-_routineOptions.CheckTransferRefundsFromMinutes);
                var transferRefundIds = await GetNextTransferRefundIdsAsync(expiredDate, skip, take, token);

                while (transferRefundIds.Any())
                {
                    foreach (var transferRefundId in transferRefundIds)
                    {
                        _mediatr.Post(new CheckTransferRefundCommand(request.RequestUser)
                        {
                            TransferRefundId = transferRefundId
                        });
                    }

                    skip += take;
                    transferRefundIds = await GetNextTransferRefundIdsAsync(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckTransferRefundCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var transferRefund = await _context.GetByIdAsync<TransferRefund>(request.TransferRefundId, token);
                if (transferRefund.Status != TransactionStatus.Created && transferRefund.Status != TransactionStatus.Waiting)
                    return Ok(false);

                if (transferRefund.CreatedOn.AddMinutes(_routineOptions.CheckTransferRefundExpiredFromMinutes) < DateTimeOffset.UtcNow && transferRefund.Status == TransactionStatus.Waiting)
                    return await _mediatr.Process(new ExpireTransferRefundCommand(request.RequestUser) { TransferRefundId = request.TransferRefundId }, token);

                var result = await _mediatr.Process(new RefreshTransferRefundStatusCommand(request.RequestUser, transferRefund.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ExpireTransferRefundCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var transferRefund = await _context.GetByIdAsync<TransferRefund>(request.TransferRefundId, token);
                transferRefund.SetStatus(TransactionStatus.Expired);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshTransferRefundStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var transferRefund = await _context.GetSingleAsync<TransferRefund>(c => c.Identifier == request.Identifier, token);
                if (transferRefund.Status == TransactionStatus.Succeeded || transferRefund.Status == TransactionStatus.Failed)
                    return Failed<TransactionStatus>(new InvalidOperationException());

                var pspResult = await _pspService.GetRefundAsync(transferRefund.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                transferRefund.SetStatus(pspResult.Data.Status);
                transferRefund.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                transferRefund.SetExecutedOn(pspResult.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);

                switch (transferRefund.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new TransferRefundFailedEvent(request.RequestUser) { RefundId = transferRefund.Id });
                        break;
                }

                return Ok(transferRefund.Status);
            });
        }

        private async Task<IEnumerable<Guid>> GetNextTransferRefundIdsAsync(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.Refunds
                .OfType<TransferRefund>()
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
