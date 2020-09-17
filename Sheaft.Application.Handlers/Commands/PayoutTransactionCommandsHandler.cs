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

namespace Sheaft.Application.Handlers
{
    public class PayoutTransactionCommandsHandler : ResultsHandler,
        IRequestHandler<CreatePayoutTransactionCommand, Result<Guid>>,
        IRequestHandler<SetPayoutSucceededCommand, Result<bool>>,
        IRequestHandler<SetPayoutFailedCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;

        public PayoutTransactionCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ILogger<PayoutTransactionCommandsHandler> logger) : base(logger)
        {
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

        public Task<Result<bool>> Handle(SetPayoutSucceededCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(SetPayoutFailedCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
