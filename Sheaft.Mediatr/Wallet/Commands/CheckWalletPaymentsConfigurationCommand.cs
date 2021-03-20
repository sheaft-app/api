using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Wallet.Commands
{
    public class CheckWalletPaymentsConfigurationCommand : Command
    {
        [JsonConstructor]
        public CheckWalletPaymentsConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }

    public class CheckWalletPaymentsConfigurationCommandHandler : CommandsHandler,
        IRequestHandler<CheckWalletPaymentsConfigurationCommand, Result>
    {
        private readonly IPspService _pspService;

        public CheckWalletPaymentsConfigurationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<CheckWalletPaymentsConfigurationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(CheckWalletPaymentsConfigurationCommand request, CancellationToken token)
        {
            var wallet =
                await _context.FindSingleAsync<Domain.Wallet>(
                    c => c.User.Id == request.UserId && c.Kind == WalletKind.Payments, token);
            if (wallet == null)
            {
                var walletResult = await _mediatr.Process(new CreateWalletPaymentsCommand(request.RequestUser)
                {
                    UserId = request.UserId
                }, token);

                if (!walletResult.Succeeded)
                    return Failure(walletResult.Exception);
            }
            else if (string.IsNullOrWhiteSpace(wallet.Identifier))
            {
                var result = await _pspService.CreateWalletAsync(wallet, token);
                if (!result.Succeeded)
                    return Failure(result.Exception);

                wallet.SetIdentifier(result.Data);
                await _context.SaveChangesAsync(token);
            }

            return Success();
        }
    }
}