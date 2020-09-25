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

namespace Sheaft.Application.Handlers
{
    public class RefundCommandsHandler : ResultsHandler,
        IRequestHandler<RefreshPayinRefundStatusCommand, Result<TransactionStatus>>,
        IRequestHandler<RefreshTransferRefundStatusCommand, Result<TransactionStatus>>,
        IRequestHandler<RefreshPayoutRefundStatusCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;

        public RefundCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<TransferCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayinRefundStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var payinRefund = await _context.GetSingleAsync<PayinRefund>(c => c.Identifier == request.Identifier, token);
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

        public async Task<Result<TransactionStatus>> Handle(RefreshTransferRefundStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transferRefund = await _context.GetSingleAsync<TransferRefund>(c => c.Identifier == request.Identifier, token);
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

        public async Task<Result<TransactionStatus>> Handle(RefreshPayoutRefundStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var pspResult = await _pspService.GetRefundAsync(request.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                switch (pspResult.Data.Status)
                {
                    case TransactionStatus.Failed:
                        await _mediatr.Post(new PayoutRefundFailedEvent(request.RequestUser) { RefundIdentifier = request.Identifier }, token);
                        break;
                    case TransactionStatus.Succeeded:
                        await _mediatr.Post(new PayoutRefundSucceededEvent(request.RequestUser) { RefundIdentifier = request.Identifier }, token);
                        break;
                }

                return Ok(pspResult.Data.Status);
            });
        }
    }
}
