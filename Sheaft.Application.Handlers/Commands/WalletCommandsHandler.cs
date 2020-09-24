using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using System;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Handlers
{
    public class WalletCommandsHandler : ResultsHandler,
           IRequestHandler<CreatePaymentsWalletCommand, Result<Guid>>,
           IRequestHandler<CreateRefundsWalletCommand, Result<Guid>>,
           IRequestHandler<CreateReturnablesWalletCommand, Result<Guid>>,
           IRequestHandler<EnsurePaymentsWalletConfiguredCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public WalletCommandsHandler(
            IMediator mediatr,
            IAppDbContext context,
            IQueueService queueService,
            IPspService pspService,
            ILogger<WalletCommandsHandler> logger)
            : base(mediatr, context, queueService, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreatePaymentsWalletCommand request, CancellationToken token)
        {
            return await CreateWalletAsync(request.UserId, "Paiements", WalletKind.Payments, token);
        }

        public async Task<Result<Guid>> Handle(CreateRefundsWalletCommand request, CancellationToken token)
        {
            return await CreateWalletAsync(request.UserId, "Remboursements", WalletKind.Refunds, token);
        }

        public async Task<Result<Guid>> Handle(CreateReturnablesWalletCommand request, CancellationToken token)
        {
            return await CreateWalletAsync(request.UserId, "Consignes", WalletKind.Returnables, token);
        }

        public async Task<Result<bool>> Handle(EnsurePaymentsWalletConfiguredCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var wallet = await _context.FindSingleAsync<Wallet>(c => c.User.Id == request.UserId && c.Kind == WalletKind.Payments, token);
                if (wallet == null)
                {
                    var walletResult = await _mediatr.Send(new CreatePaymentsWalletCommand(request.RequestUser)
                    {
                        UserId = request.UserId
                    }, token);

                    if (!walletResult.Success)
                        return Failed<bool>(walletResult.Exception);
                }
                else if (string.IsNullOrWhiteSpace(wallet.Identifier))
                {
                    var result = await _pspService.CreateWalletAsync(wallet, token);
                    if (!result.Success)
                        return Failed<bool>(result.Exception);

                    wallet.SetIdentifier(result.Data);

                    _context.Update(wallet);
                    await _context.SaveChangesAsync(token);
                }

                return Ok(true);
            });
        }

        private async Task<Result<Guid>> CreateWalletAsync(Guid userId, string name, WalletKind kind, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(userId, token);

                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var wallet = new Wallet(Guid.NewGuid(), name, kind, user);
                    await _context.AddAsync(wallet, token);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreateWalletAsync(wallet, token);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(token);
                        return Failed<Guid>(result.Exception);
                    }

                    wallet.SetIdentifier(result.Data);

                    _context.Update(wallet);
                    await _context.SaveChangesAsync(token);

                    await transaction.CommitAsync(token);
                    return Ok(wallet.Id);
                }
            });
        }
    }
}