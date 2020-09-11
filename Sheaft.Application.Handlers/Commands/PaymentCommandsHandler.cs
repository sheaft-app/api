﻿using Sheaft.Application.Commands;
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
    public class PaymentCommandsHandler : CommandsHandler,
           IRequestHandler<CreateBankAccountCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;

        public PaymentCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ILogger<PaymentCommandsHandler> logger) : base(logger)
        {
            _context = context;
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateBankAccountCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.UserId, token);

                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var address = request.Address != null ?
                           new BankAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City,
                           request.Address.Country)
                           : null;

                    var bankAccount = new BankAccount(Guid.NewGuid(), request.Name, request.Owner, request.IBAN, request.BIC, address, user);
                    await _context.AddAsync(bankAccount, token);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreateBankIbanAsync(bankAccount, token);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(token);
                        return Failed<Guid>(result.Exception);
                    }

                    bankAccount.SetIdentifier(result.Data);

                    await transaction.CommitAsync(token);
                    return Ok(bankAccount.Id);
                }
            });
        }
    }
}