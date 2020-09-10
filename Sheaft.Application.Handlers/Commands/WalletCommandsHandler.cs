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

namespace Sheaft.Application.Handlers
{
    public class WalletCommandsHandler : CommandsHandler,
           IRequestHandler<CreateWalletCommand, Result<Guid>>
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

        public async Task<Result<Guid>> Handle(CreateWalletCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.UserId, token);

                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var wallet = new Wallet(Guid.NewGuid(), request.Name, request.Kind);
                    user.AddWallet(wallet);

                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreateWalletForUserAsync(wallet, user, token);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(token);
                        return Failed<Guid>(result.Exception);
                    }

                    wallet.SetIdentifier(result.Data);

                    await transaction.CommitAsync(token);
                    return Ok(wallet.Id);
                }
            });
        }
    }
}