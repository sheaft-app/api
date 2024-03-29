﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Bank.Commands
{
    public class UpdateBankAccountCommand : Command
    {
        protected UpdateBankAccountCommand()
        {
            
        }
        [JsonConstructor]
        public UpdateBankAccountCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid BankAccountId { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public AddressDto Address { get; set; }
    }

    public class UpdateBankAccountCommandHandler : CommandsHandler,
        IRequestHandler<UpdateBankAccountCommand, Result>
    {
        private readonly IPspService _pspService;

        public UpdateBankAccountCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<UpdateBankAccountCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(UpdateBankAccountCommand request, CancellationToken token)
        {
            var bankAccount = await _context.BankAccounts.SingleAsync(e => e.Id == request.BankAccountId, token);

            var address = request.Address != null
                ? new BankAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                    request.Address.City,
                    request.Address.Country)
                : null;

            if (!string.IsNullOrWhiteSpace(bankAccount.Identifier))
            {
                var resetResult = await _pspService.UpdateBankIbanAsync(bankAccount, false, token);
                if (!resetResult.Succeeded)
                    return Failure(resetResult);

                bankAccount.SetIdentifier(string.Empty);
                await _context.SaveChangesAsync(token);
            }

            bankAccount.SetAddress(address);
            bankAccount.SetName(request.Name);
            bankAccount.SetOwner(request.Owner);
            bankAccount.SetIban(request.IBAN);
            bankAccount.SetBic(request.BIC);

            var result = await _pspService.CreateBankIbanAsync(bankAccount, token);
            if (!result.Succeeded)
                return Failure(result);

            bankAccount.SetIdentifier(result.Data);
            await _context.SaveChangesAsync(token);

            return Success();
        }
    }
}