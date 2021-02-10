using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CreatePayoutCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePayoutCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
        public List<Guid> TransferIds { get; set; }
    }
    
    public class CreatePayoutCommandHandler : CommandsHandler,
        IRequestHandler<CreatePayoutCommand, Result<Guid>>
    {
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;
        private readonly IPspService _pspService;

        public CreatePayoutCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreatePayoutCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }
        public async Task<Result<Guid>> Handle(CreatePayoutCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producerLegals = await _context.GetSingleAsync<BusinessLegal>(c => c.User.Id == request.ProducerId, token);           
                if (producerLegals.Validation != LegalValidation.Regular)
                    return Failed<Guid>(new BadRequestException(MessageKind.Payout_CannotCreate_User_NotValidated));

                var wallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.ProducerId, token);
                var bankAccount = await _context.GetSingleAsync<BankAccount>(c => c.User.Id == request.ProducerId && c.IsActive, token);

                var transfers = await _context.GetAsync<Transfer>(
                    t => request.TransferIds.Contains(t.Id)
                        && t.PurchaseOrder.Status == PurchaseOrderStatus.Delivered
                        && (t.Payout == null || t.Payout.Status == TransactionStatus.Failed), 
                    token);

                var pendingWithholdings = await _context.GetAsync<Withholding>(c => c.DebitedWallet.User.Id == request.ProducerId && c.Status == TransactionStatus.Waiting && (c.Payout == null || c.Payout.Status == TransactionStatus.Failed), token);
                var withholdings = new List<Withholding>();
                var amount = transfers.Sum(t => t.Credited);
                var holdingAmount = 0m;
                foreach(var withholding in pendingWithholdings.OrderBy(w => w.Debited))
                {
                    holdingAmount += withholding.Debited;
                    if (holdingAmount < amount)
                        withholdings.Add(withholding);
                    else break;
                }

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var payout = new Payout(Guid.NewGuid(), wallet, bankAccount, transfers, withholdings);
                    await _context.AddAsync(payout);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreatePayoutAsync(payout, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    payout.SetIdentifier(result.Data.Identifier);
                    payout.SetStatus(result.Data.Status);
                    payout.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    payout.SetExecutedOn(result.Data.ProcessedOn);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(payout.Id);
                }
            });
        }
    }
}
