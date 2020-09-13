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
    public class TransactionCommandsHandler : CommandsHandler,
        IRequestHandler<CreateWebPayInTransactionCommand, Result<Guid>>,
        IRequestHandler<CreateTransferTransactionCommand, Result<Guid>>,
        IRequestHandler<CreatePayoutTransactionCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;

        public TransactionCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ILogger<TransactionCommandsHandler> logger) : base(logger)
        {
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

        public async Task<Result<Guid>> Handle(CreateTransferTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var debitedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.FromUserId, token);
                var creditedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.ToUserId, token);
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.PurchaseOrderId, token);

                var transfer = new TransferTransaction(Guid.NewGuid(), debitedWallet, creditedWallet, purchaseOrder);

                await _context.AddAsync(transfer, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateTransferAsync(transfer, token);
                if (!result.Success)
                {
                    return Failed<Guid>(result.Exception);
                }

                transfer.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                transfer.SetIdentifier(result.Data.Identifier);
                transfer.SetStatus(result.Data.Status);
                transfer.SetCreditedAmount(result.Data.Credited);

                _context.Update(transfer);

                await _context.SaveChangesAsync(token);
                return Ok(transfer.Id);
            });
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
    }
}
