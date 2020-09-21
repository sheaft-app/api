using Sheaft.Application.Commands;
using Sheaft.Infrastructure.Interop;
using System;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Domain.Models;
using Sheaft.Services.Interop;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Handlers
{
    public class WalletCommandsHandler : ResultsHandler,
           IRequestHandler<CreatePaymentsWalletCommand, Result<Guid>>,
           IRequestHandler<CreateRefundsWalletCommand, Result<Guid>>,
           IRequestHandler<CreateReturnablesWalletCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;

        public WalletCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ILogger<WalletCommandsHandler> logger) : base(logger)
        {
            _context = context;
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreatePaymentsWalletCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () => await CreateWalletAsync(request.UserId, "Paiements", WalletKind.Payments, token));
        }

        public async Task<Result<Guid>> Handle(CreateRefundsWalletCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () => await CreateWalletAsync(request.UserId, "Remboursements", WalletKind.Refunds, token));
        }

        public async Task<Result<Guid>> Handle(CreateReturnablesWalletCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () => await CreateWalletAsync(request.UserId, "Consignes", WalletKind.Returnables, token));
        }

        private async Task<Result<Guid>> CreateWalletAsync(Guid userId, string name, WalletKind kind, CancellationToken token)
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
        }
    }
}