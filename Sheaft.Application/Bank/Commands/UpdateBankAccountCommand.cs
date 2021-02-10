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
    public class UpdateBankAccountCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateBankAccountCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public AddressInput Address { get; set; }
    }
    
    public class UpdateBankAccountCommandHandler : CommandsHandler,
           IRequestHandler<UpdateBankAccountCommand, Result<bool>>
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

        public async Task<Result<bool>> Handle(UpdateBankAccountCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var bankAccount = await _context.GetByIdAsync<BankAccount>(request.Id, token);

                var address = request.Address != null ?
                       new BankAddress(request.Address.Line1, request.Address.Line2, request.Address.Zipcode, request.Address.City,
                       request.Address.Country)
                       : null;

                if (!string.IsNullOrWhiteSpace(bankAccount.Identifier))
                {
                    var resetResult = await _pspService.UpdateBankIbanAsync(bankAccount, false, token);
                    if (!resetResult.Success)
                        return Failed<bool>(resetResult.Exception);

                    bankAccount.SetIdentifier(string.Empty);
                    await _context.SaveChangesAsync(token);
                }

                bankAccount.SetAddress(address);
                bankAccount.SetName(request.Name);
                bankAccount.SetOwner(request.Owner);
                bankAccount.SetIban(request.IBAN);
                bankAccount.SetBic(request.BIC);

                var result = await _pspService.CreateBankIbanAsync(bankAccount, token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                bankAccount.SetIdentifier(result.Data);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}
