using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;

namespace Sheaft.Application.Bank.Commands
{
    public class CreateBankAccountCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateBankAccountCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public AddressInput Address { get; set; }
    }

    public class CreateBankAccountCommandHandler : CommandsHandler,
        IRequestHandler<CreateBankAccountCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public CreateBankAccountCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<CreateBankAccountCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateBankAccountCommand request, CancellationToken token)
        {
            var user = await _context.GetByIdAsync<Domain.User>(request.UserId, token);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var address = request.Address != null
                    ? new BankAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode,
                        request.Address.City,
                        request.Address.Country)
                    : null;

                var bankAccount = new BankAccount(Guid.NewGuid(), request.Name, request.Owner, request.IBAN,
                    request.BIC, address, user);
                await _context.AddAsync(bankAccount, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateBankIbanAsync(bankAccount, token);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync(token);
                    return Failure<Guid>(result.Exception);
                }

                bankAccount.SetIdentifier(result.Data);
                await _context.SaveChangesAsync(token);

                await transaction.CommitAsync(token);
                return Success(bankAccount.Id);
            }
        }
    }
}