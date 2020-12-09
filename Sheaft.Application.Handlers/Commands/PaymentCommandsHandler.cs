using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using System;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Handlers
{
    public class PaymentCommandsHandler : ResultsHandler,
           IRequestHandler<CreateBankAccountCommand, Result<Guid>>,
           IRequestHandler<UpdateBankAccountCommand, Result<bool>>
    {
        private readonly IPspService _pspService;

        public PaymentCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<PaymentCommandsHandler> logger)
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