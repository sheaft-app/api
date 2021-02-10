using Sheaft.Core;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CreateWithholdingCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateWithholdingCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
    
    public class CreateWithholdingCommandHandler : CommandsHandler,
        IRequestHandler<CreateWithholdingCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;
        private readonly PspOptions _pspOptions;

        public CreateWithholdingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreateWithholdingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateWithholdingCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var debitedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.UserId, token);
                var creditedWallet = await _context.GetSingleAsync<Wallet>(c => c.Identifier == _pspOptions.DocumentWalletId, token);

                var withholding = new Withholding(Guid.NewGuid(), request.Amount, debitedWallet, creditedWallet);
                await _context.AddAsync(withholding, token);
                await _context.SaveChangesAsync(token);

                return Ok(withholding.Id);
            });
        }
    }
}
