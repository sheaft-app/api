using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Payout.Commands
{
    public class CreatePayoutCommand : Command<Guid>
    {
        protected CreatePayoutCommand()
        {
            
        }
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
        private readonly IPspService _pspService;

        public CreatePayoutCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<CreatePayoutCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreatePayoutCommand request, CancellationToken token)
        {
            var producerLegals =
                await _context.Set<BusinessLegal>().SingleOrDefaultAsync(c => c.UserId == request.ProducerId, token);
            if (producerLegals.Validation != LegalValidation.Regular)
                return Failure<Guid>(SheaftException.BadRequest(MessageKind.Payout_CannotCreate_User_NotValidated));

            var wallet = await _context.Wallets.SingleOrDefaultAsync(c => c.UserId == request.ProducerId, token);
            var bankAccount =
                await _context.BankAccounts.SingleOrDefaultAsync(c => c.UserId == request.ProducerId && c.IsActive,
                    token);

            var transfers = await _context.Transfers
                .Where(t => request.TransferIds.Contains(t.Id)
                            && t.PurchaseOrder.Status == PurchaseOrderStatus.Delivered
                            && (t.Payout == null || t.Payout.Status == TransactionStatus.Failed))
                .ToListAsync(token);

            var pendingWithholdings = await _context.Withholdings
                .Where(c =>
                    c.DebitedWallet.UserId == request.ProducerId && c.Status == TransactionStatus.Waiting &&
                    (!c.PayoutId.HasValue || c.Payout.Status == TransactionStatus.Failed))
                .ToListAsync(token);
            var withholdings = new List<Domain.Withholding>();
            var amount = transfers.Sum(t => t.Credited);
            var holdingAmount = 0m;
            foreach (var withholding in pendingWithholdings.OrderBy(w => w.Debited))
            {
                holdingAmount += withholding.Debited;
                if (holdingAmount < amount)
                    withholdings.Add(withholding);
                else break;
            }

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var payout = new Domain.Payout(Guid.NewGuid(), wallet, bankAccount, transfers, withholdings);
                await _context.AddAsync(payout);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreatePayoutAsync(payout, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result);

                payout.SetIdentifier(result.Data.Identifier);
                payout.SetStatus(result.Data.Status);
                payout.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                payout.SetExecutedOn(result.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return Success(payout.Id);
            }
        }
    }
}