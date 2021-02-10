using Sheaft.Core;
using Newtonsoft.Json;
using System;
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
        private readonly RoutineOptions _routineOptions;
        private readonly PspOptions _pspOptions;

        public CreateDonationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CreateDonationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateDonationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                if (order.Payin == null
                    || order.Payin.Status != TransactionStatus.Succeeded
                    || (order.Donation != null && order.Donation.Status != TransactionStatus.Failed))
                    return BadRequest<Guid>(MessageKind.Donation_CannotCreate_AlreadySucceeded);

                var orderDonations = await _context.FindAsync<Donation>(c => c.Order.Id == order.Id, token);
                if (orderDonations.Any(c => c.Status != TransactionStatus.Failed))
                    return BadRequest<Guid>(MessageKind.Donation_CannotCreate_PendingDonation);

                var creditedWallet =
                    await _context.GetSingleAsync<Wallet>(c => c.Identifier == _pspOptions.WalletId, token);
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var donation = new Donation(Guid.NewGuid(), order.Payin.CreditedWallet, creditedWallet, order);
                    await _context.AddAsync(donation, token);
                    await _context.SaveChangesAsync(token);

                    order.SetDonation(donation);

                    var result = await _pspService.CreateDonationAsync(donation, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    donation.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    donation.SetIdentifier(result.Data.Identifier);
                    donation.SetStatus(result.Data.Status);
                    donation.SetExecutedOn(result.Data.ProcessedOn);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(donation.Id);
                }
            });
        }
    }
}
