using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.Donation.Commands
{
    public class CreateDonationCommand : Command<Guid>
    {
        protected CreateDonationCommand()
        {
            
        }
        [JsonConstructor]
        public CreateDonationCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }

    public class CreateDonationCommandHandler : CommandsHandler,
        IRequestHandler<CreateDonationCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;
        private readonly PspOptions _pspOptions;

        public CreateDonationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreateDonationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateDonationCommand request, CancellationToken token)
        {
            var order = await _context.Orders.SingleAsync(e => e.Id == request.OrderId, token);
            var orderPayin = await _context.Payins
                .SingleOrDefaultAsync(c => c.OrderId == order.Id && c.Status == TransactionStatus.Succeeded, token);

            var pendingDonations = await _context.Donations
                .Where(t => t.OrderId == request.OrderId)
                .ToListAsync(token);

            if (pendingDonations.Any(pt => pt.Status == TransactionStatus.Succeeded))
                return Failure<Guid>(MessageKind.Donation_CannotCreate_AlreadySucceeded);

            if (pendingDonations.Any(pt =>
                pt.Status == TransactionStatus.Created || pt.Status == TransactionStatus.Waiting))
                return Failure<Guid>(MessageKind.Donation_CannotCreate_PendingDonation);

            var sheaftWallet = await _context.Wallets
                .SingleOrDefaultAsync(c => c.Identifier == _pspOptions.WalletId, token);
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var donation = new Domain.Donation(Guid.NewGuid(), orderPayin.CreditedWallet, sheaftWallet, order);
                await _context.AddAsync(donation, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateDonationAsync(donation, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result);

                donation.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                donation.SetIdentifier(result.Data.Identifier);
                donation.SetStatus(result.Data.Status);
                donation.SetExecutedOn(result.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return Success(donation.Id);
            }
        }
    }
}