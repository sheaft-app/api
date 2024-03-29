﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Wallet.Commands
{
    public class CreateWalletPaymentsCommand : Command<Guid>
    {
        protected CreateWalletPaymentsCommand()
        {
            
        }
        [JsonConstructor]
        public CreateWalletPaymentsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
    
    public class CreateWalletPaymentsCommandHandler : CommandsHandler,
           IRequestHandler<CreateWalletPaymentsCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public CreateWalletPaymentsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<CreateWalletPaymentsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateWalletPaymentsCommand request, CancellationToken token)
        {
            return await CreateWalletAsync(request.UserId, "Paiements", WalletKind.Payments, token);
        }

        private async Task<Result<Guid>> CreateWalletAsync(Guid userId, string name, WalletKind kind, CancellationToken token)
        {
            var user = await _context.Users.SingleAsync(e => e.Id == userId, token);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var wallet = new Domain.Wallet(Guid.NewGuid(), name, kind, user);
                await _context.AddAsync(wallet, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateWalletAsync(wallet, token);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync(token);
                    return Failure<Guid>(result);
                }

                wallet.SetIdentifier(result.Data);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return Success(wallet.Id);
            }
        }
    }
}
