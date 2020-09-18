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
    public class PayoutTransactionCommandsHandler : ResultsHandler,
        IRequestHandler<CreatePayoutTransactionCommand, Result<Guid>>,
        IRequestHandler<SetPayoutStatusCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;
        private readonly IMediator _mediatr;

        public PayoutTransactionCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            IMediator mediatr,
            ILogger<PayoutTransactionCommandsHandler> logger) : base(logger)
        {
            _mediatr = mediatr;
            _context = context;
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreatePayoutTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var debitedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.FromUserId, token);
                var bankAccount = await _context.GetSingleAsync<BankAccount>(c => c.Id == request.BankAccountId && c.IsActive, token);

                var payout = new PayoutTransaction(Guid.NewGuid(), request.Amount, debitedWallet, bankAccount);

                await _context.AddAsync(payout, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreatePayoutAsync(payout, token);
                if (!result.Success)
                {
                    return Failed<Guid>(result.Exception);
                }

                payout.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                payout.SetStatus(result.Data.Status);
                payout.SetCreditedAmount(result.Data.Credited);

                _context.Update(payout);

                await _context.SaveChangesAsync(token);
                return Ok(payout.Id);
            });
        }

        public async Task<Result<bool>> Handle(SetPayoutStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var transaction = await _context.GetSingleAsync<PayoutTransaction>(c => c.Identifier == request.Identifier, token);
                var pspResult = await _pspService.GetPayoutAsync(transaction.Identifier, token);
                if (!pspResult.Success)
                    return Failed<bool>(pspResult.Exception);

                transaction.SetStatus(pspResult.Data.Status);
                transaction.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                transaction.SetProcessedOn(pspResult.Data.ProcessedOn);

                _context.Update(transaction);
                var success = await _context.SaveChangesAsync(token) > 0;

                switch (request.Kind)
                {
                    case PspEventKind.PAYOUT_NORMAL_FAILED:
                        await _mediatr.Publish(new PayoutFailedEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                    case PspEventKind.PAYOUT_NORMAL_SUCCEEDED:
                        await _mediatr.Publish(new PayoutSucceededEvent(request.RequestUser) { TransactionId = transaction.Id }, token);
                        break;
                }

                return Ok(success);
            });
        }
    }
}
