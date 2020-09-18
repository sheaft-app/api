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
    public class PayinTransactionCommandsHandler : ResultsHandler,
        IRequestHandler<CreateWebPayInTransactionCommand, Result<Guid>>,
        IRequestHandler<SetPayinStatusCommand, Result<bool>>,
        IRequestHandler<SetRefundPayinStatusCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;
        private readonly IMediator _mediatr;

        public PayinTransactionCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            IMediator mediatr,
            ILogger<PayinTransactionCommandsHandler> logger) : base(logger)
        {
            _mediatr = mediatr;
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
                var transaction = await _context.GetSingleAsync<Transaction>(c => c.Identifier == request.Identifier, token);
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
                        await _mediatr.Publish(new PayinFailedEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                    case PspEventKind.PAYIN_NORMAL_SUCCEEDED:
                        await _mediatr.Publish(new PayinSucceededEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
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
                        await _mediatr.Publish(new RefundPayinFailedEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                    case PspEventKind.PAYIN_REFUND_SUCCEEDED:
                        await _mediatr.Publish(new RefundPayinSucceededEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                }

                return Ok(success);
            });
        }
    }
}
