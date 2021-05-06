using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Options;

namespace Sheaft.Mediatr.Withholding.Commands
{
    public class CreateWithholdingCommand : Command<Guid>
    {
        protected CreateWithholdingCommand()
        {
            
        }
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
        private readonly PspOptions _pspOptions;

        public CreateWithholdingCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreateWithholdingCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateWithholdingCommand request, CancellationToken token)
        {
            var debitedWallet = await _context.Wallets
                .SingleOrDefaultAsync(c => c.UserId == request.UserId, token);
            var creditedWallet = await _context.Wallets
                .SingleOrDefaultAsync(c => c.Identifier == _pspOptions.DocumentWalletId, token);

            var withholding = new Domain.Withholding(Guid.NewGuid(), request.Amount, debitedWallet, creditedWallet);
            await _context.AddAsync(withholding, token);
            await _context.SaveChangesAsync(token);

            return Success(withholding.Id);
        }
    }
}