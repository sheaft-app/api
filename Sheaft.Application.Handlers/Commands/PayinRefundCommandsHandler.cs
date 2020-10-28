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
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Sheaft.Exceptions;

namespace Sheaft.Application.Handlers
{
    public class PayinRefundCommandsHandler : ResultsHandler,
        IRequestHandler<CheckNewPayinRefundsCommand, Result<bool>>,
        IRequestHandler<CheckPayinRefundsCommand, Result<bool>>,
        IRequestHandler<CheckPayinRefundCommand, Result<bool>>,
        IRequestHandler<RefreshPayinRefundStatusCommand, Result<TransactionStatus>>,
        IRequestHandler<CreatePayinRefundCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public PayinRefundCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<PayinRefundCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckNewPayinRefundsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-_routineOptions.CheckNewPayinRefundsFromMinutes);
                var payinToRefundIds = await GetNextNewPayinRefundIdsAsync(expiredDate, skip, take, token);
                while (payinToRefundIds.Any())
                {
                    foreach (var payinToRefundId in payinToRefundIds)
                    {
                        _mediatr.Post(new CreatePayinRefundCommand(request.RequestUser)
                        {
                            PayinId = payinToRefundId
                        });
                    }

                    skip += take;
                    payinToRefundIds = await GetNextNewPayinRefundIdsAsync(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckPayinRefundsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var skip = 0;
                const int take = 100;

                var payinRefundIds = await GetNextPayinRefundIdsAsync(skip, take, token);
                while (payinRefundIds.Any())
                {
                    foreach (var payinRefundId in payinRefundIds)
                    {
                        _mediatr.Post(new CheckPayinRefundCommand(request.RequestUser)
                        {
                            PayinRefundId = payinRefundId
                        });
                    }

                    skip += take;
                    payinRefundIds = await GetNextPayinRefundIdsAsync(skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckPayinRefundCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payinRefund = await _context.GetByIdAsync<PayinRefund>(request.PayinRefundId, token);
                if (payinRefund.Status != TransactionStatus.Created && payinRefund.Status != TransactionStatus.Waiting)
                    return Ok(false);

                var result = await _mediatr.Process(new RefreshPayinRefundStatusCommand(request.RequestUser, payinRefund.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayinRefundStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payinRefund = await _context.GetSingleAsync<PayinRefund>(c => c.Identifier == request.Identifier, token);
                if (payinRefund.Status == TransactionStatus.Succeeded || payinRefund.Status == TransactionStatus.Failed)
                    return Ok(payinRefund.Status);

                var pspResult = await _pspService.GetRefundAsync(payinRefund.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                payinRefund.SetStatus(pspResult.Data.Status);
                payinRefund.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                payinRefund.SetExecutedOn(pspResult.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);

                switch (payinRefund.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new PayinRefundFailedEvent(request.RequestUser) { RefundId = payinRefund.Id });
                        break;
                }

                return Ok(payinRefund.Status);
            });
        }

        public async Task<Result<Guid>> Handle(CreatePayinRefundCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payin = await _context.GetByIdAsync<Payin>(request.PayinId, token);
                if (payin.Id != payin.Order.Payin.Id)
                    return BadRequest<Guid>(MessageKind.PayinRefund_CannotCreate_Order_Payin_HasChanged);

                if (payin.Status != TransactionStatus.Succeeded)
                    return BadRequest<Guid>(MessageKind.PayinRefund_CannotCreate_Payin_Invalid_Status);

                if(payin.Refund != null && payin.Refund.Status != TransactionStatus.Failed)
                    return BadRequest<Guid>(MessageKind.PayinRefund_CannotCreate_Pending_PayinRefund);

                var purchaseOrdersToRefund = payin.Order.PurchaseOrders.Where(po => po.Status > PurchaseOrderStatus.Delivered);
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var payinRefund = new PayinRefund(Guid.NewGuid(), payin, purchaseOrdersToRefund.Sum(po => po.TotalOnSalePrice));

                    await _context.AddAsync(payinRefund, token);
                    await _context.SaveChangesAsync(token);

                    payin.SetRefund(payinRefund);

                    var result = await _pspService.RefundPayinAsync(payinRefund, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    payinRefund.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    payinRefund.SetIdentifier(result.Data.Identifier);
                    payinRefund.SetStatus(result.Data.Status);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(payinRefund.Id);
                }
            });
        }

        private async Task<IEnumerable<Guid>> GetNextPayinRefundIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Refunds
                .OfType<PayinRefund>()
                .Get(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created, true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }

        private async Task<IEnumerable<Guid>> GetNextNewPayinRefundIdsAsync(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.Orders
                .Get(c =>
                        c.Payin != null
                        && c.Payin.Status == TransactionStatus.Succeeded
                        && (c.Payin.Refund == null || c.Payin.Status == TransactionStatus.Failed)
                        && c.PurchaseOrders.All(po => po.Status >= PurchaseOrderStatus.Delivered)
                        && c.PurchaseOrders.Any(po => po.WithdrawnOn.HasValue)
                        && c.PurchaseOrders.Max(po => po.WithdrawnOn) < expiredDate, true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Payin.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
