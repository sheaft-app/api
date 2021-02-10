using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Donation.Commands
{
    public class CreateDonationCommand : Command<Guid>
    {
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
            var order = await _context.GetByIdAsync<Domain.Order>(request.OrderId, token);
            if (order.Payin == null
                || order.Payin.Status != TransactionStatus.Succeeded
                || (order.Donation != null && order.Donation.Status != TransactionStatus.Failed))
                return Failure<Guid>(MessageKind.Donation_CannotCreate_AlreadySucceeded);

            var orderDonations = await _context.FindAsync<Domain.Donation>(c => c.Order.Id == order.Id, token);
            if (orderDonations.Any(c => c.Status != TransactionStatus.Failed))
                return Failure<Guid>(MessageKind.Donation_CannotCreate_PendingDonation);

            var creditedWallet =
                await _context.GetSingleAsync<Domain.Wallet>(c => c.Identifier == _pspOptions.WalletId, token);
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var donation = new Domain.Donation(Guid.NewGuid(), order.Payin.CreditedWallet, creditedWallet, order);
                await _context.AddAsync(donation, token);
                await _context.SaveChangesAsync(token);

                order.SetDonation(donation);

                var result = await _pspService.CreateDonationAsync(donation, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result.Exception);

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