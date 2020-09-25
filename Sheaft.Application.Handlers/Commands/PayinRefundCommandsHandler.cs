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
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class PayinRefundCommandsHandler : ResultsHandler,
        IRequestHandler<CheckPayinRefundsCommand, Result<bool>>,
        IRequestHandler<CheckPayinRefundCommand, Result<bool>>,
        IRequestHandler<ExpirePayinRefundCommand, Result<bool>>,
        IRequestHandler<RefreshPayinRefundStatusCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;

        public PayinRefundCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<PayinRefundCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<bool>> Handle(CheckPayinRefundsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-60);
                var payinRefundIds = await GetNextPayinRefundIdsAsync(expiredDate, skip, take, token);

                while (payinRefundIds.Any())
                {
                    foreach (var payinRefundId in payinRefundIds)
                    {
                        await _mediatr.Post(new CheckPayinRefundCommand(request.RequestUser)
                        {
                            PayinRefundId = payinRefundId
                        }, token);
                    }

                    skip += take;
                    payinRefundIds = await GetNextPayinRefundIdsAsync(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckPayinRefundCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transfer = await _context.GetByIdAsync<PayinRefund>(request.PayinRefundId, token);
                if (transfer.Status != TransactionStatus.Created && transfer.Status != TransactionStatus.Waiting)
                    return Ok(false);

                if (transfer.CreatedOn.AddMinutes(1440) < DateTimeOffset.UtcNow && transfer.Status == TransactionStatus.Waiting)
                    return await _mediatr.Process(new ExpirePayinRefundCommand(request.RequestUser) { PayinRefundId = request.PayinRefundId }, token);

                var result = await _mediatr.Process(new RefreshPayinRefundStatusCommand(request.RequestUser, transfer.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ExpirePayinRefundCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var payinRefund = await _context.GetByIdAsync<PayinRefund>(request.PayinRefundId, token);
                payinRefund.SetStatus(TransactionStatus.Expired);

                _context.Update(payinRefund);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayinRefundStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var payinRefund = await _context.GetSingleAsync<PayinRefund>(c => c.Identifier == request.Identifier, token);
                if (payinRefund.Status == TransactionStatus.Succeeded || payinRefund.Status == TransactionStatus.Failed)
                    return Failed<TransactionStatus>(new InvalidOperationException());

                var pspResult = await _pspService.GetRefundAsync(payinRefund.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                payinRefund.SetStatus(pspResult.Data.Status);
                payinRefund.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                payinRefund.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(payinRefund);
                await _context.SaveChangesAsync(token);

                switch (payinRefund.Status)
                {
                    case TransactionStatus.Failed:
                        await _mediatr.Post(new PayinRefundFailedEvent(request.RequestUser) { RefundId = payinRefund.Id }, token);
                        break;
                    case TransactionStatus.Succeeded:
                        await _mediatr.Post(new PayinRefundSucceededEvent(request.RequestUser) { RefundId = payinRefund.Id }, token);
                        break;
                }

                return Ok(payinRefund.Status);
            });
        }

        private async Task<IEnumerable<Guid>> GetNextPayinRefundIdsAsync(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.Refunds
                .OfType<PayinRefund>()
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
