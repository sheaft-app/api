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

namespace Sheaft.Application.Handlers
{
    public class TransferRefundCommandsHandler : ResultsHandler,
        IRequestHandler<CheckTransferRefundsCommand, Result<bool>>,
        IRequestHandler<CheckTransferRefundCommand, Result<bool>>,
        IRequestHandler<ExpireTransferRefundCommand, Result<bool>>,
        IRequestHandler<RefreshTransferRefundStatusCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;

        public TransferRefundCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<TransferRefundCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<bool>> Handle(CheckTransferRefundsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-60);
                var transferRefundIds = await GetNextTransferRefundIdsAsync(expiredDate, skip, take, token);

                while (transferRefundIds.Any())
                {
                    foreach (var transferRefundId in transferRefundIds)
                    {
                        await _mediatr.Post(new CheckTransferRefundCommand(request.RequestUser)
                        {
                            TransferRefundId = transferRefundId
                        }, token);
                    }

                    skip += take;
                    transferRefundIds = await GetNextTransferRefundIdsAsync(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckTransferRefundCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transfer = await _context.GetByIdAsync<TransferRefund>(request.TransferRefundId, token);
                if (transfer.Status != TransactionStatus.Created && transfer.Status != TransactionStatus.Waiting)
                    return Ok(false);

                if (transfer.CreatedOn.AddMinutes(1440) < DateTimeOffset.UtcNow && transfer.Status == TransactionStatus.Waiting)
                    return await _mediatr.Process(new ExpireTransferRefundCommand(request.RequestUser) { TransferRefundId = request.TransferRefundId }, token);

                var result = await _mediatr.Process(new RefreshTransferRefundStatusCommand(request.RequestUser, transfer.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ExpireTransferRefundCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transferRefund = await _context.GetByIdAsync<TransferRefund>(request.TransferRefundId, token);
                transferRefund.SetStatus(TransactionStatus.Expired);

                _context.Update(transferRefund);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshTransferRefundStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transferRefund = await _context.GetSingleAsync<TransferRefund>(c => c.Identifier == request.Identifier, token);
                if (transferRefund.Status == TransactionStatus.Succeeded || transferRefund.Status == TransactionStatus.Failed)
                    return Failed<TransactionStatus>(new InvalidOperationException());

                var pspResult = await _pspService.GetRefundAsync(transferRefund.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                transferRefund.SetStatus(pspResult.Data.Status);
                transferRefund.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                transferRefund.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(transferRefund);
                await _context.SaveChangesAsync(token);

                switch (transferRefund.Status)
                {
                    case TransactionStatus.Failed:
                        await _mediatr.Post(new TransferRefundFailedEvent(request.RequestUser) { RefundId = transferRefund.Id }, token);
                        break;
                    case TransactionStatus.Succeeded:
                        await _mediatr.Post(new TransferRefundSucceededEvent(request.RequestUser) { RefundId = transferRefund.Id }, token);
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
