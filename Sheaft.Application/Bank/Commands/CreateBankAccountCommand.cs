using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Application.Models;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Commands
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
            return await ExecuteAsync(request, async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.UserId, token);

                using (var transaction = await _context.BeginTransactionAsync(token))
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
                    await _context.SaveChangesAsync(token);

                    await transaction.CommitAsync(token);
                    return Ok(bankAccount.Id);
                }
            });
        }
    }
}
