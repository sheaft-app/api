using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
{
    public class CheckWalletPaymentsConfigurationCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckWalletPaymentsConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
    
    public class CheckWalletPaymentsConfigurationCommandHandler : CommandsHandler,
           IRequestHandler<CheckWalletPaymentsConfigurationCommand, Result<bool>>
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

        public async Task<Result<bool>> Handle(CheckWalletPaymentsConfigurationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var wallet = await _context.FindSingleAsync<Wallet>(c => c.User.Id == request.UserId && c.Kind == WalletKind.Payments, token);
                if (wallet == null)
                {
                    var walletResult = await _mediatr.Process(new CreateWalletPaymentsCommand(request.RequestUser)
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
                    await _context.SaveChangesAsync(token);
                }

                return Ok(true);
            });
        }
    }
}
