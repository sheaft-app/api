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
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Transfer.Commands
{
    public class RefreshTransferStatusCommand : Command
    {
        [JsonConstructor]
        public RefreshTransferStatusCommand(RequestUser requestUser, string identifier) : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshTransferStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshTransferStatusCommand, Result>
    {
        private readonly IPspService _pspService;

        public RefreshTransferStatusCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<RefreshTransferStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(RefreshTransferStatusCommand request,
            CancellationToken token)
        {
            var transfer =
                await _context.Transfers.SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);
            if (transfer != null)
                return await HandleTransferStatusAsync(request, transfer, token);

            var donation =
                await _context.Donations.SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);
            if (donation != null)
                return await HandleDonationStatusAsync(request, donation, token);

            var withholding =
                await _context.Withholdings.SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);
            if (withholding != null)
                return await HandleWithholdingStatusAsync(request, withholding, token);

            return Failure(MessageKind.Unexpected);
        }

        private async Task<Result> HandleTransferStatusAsync(RefreshTransferStatusCommand request,
            Domain.Transfer transfer, CancellationToken token)
        {
            if (transfer.Status == TransactionStatus.Succeeded || transfer.Status == TransactionStatus.Failed || transfer.Status == TransactionStatus.Cancelled)
                return Success();

            var pspResult = await _pspService.GetTransferAsync(transfer.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult);

            transfer.SetStatus(pspResult.Data.Status);
            transfer.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            transfer.SetExecutedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);
            return Success();
        }

        private async Task<Result> HandleDonationStatusAsync(RefreshTransferStatusCommand request,
            Domain.Donation donation, CancellationToken token)
        {
            if (donation.Status == TransactionStatus.Succeeded || donation.Status == TransactionStatus.Failed)
                return Success();

            var pspResult = await _pspService.GetTransferAsync(donation.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult);

            donation.SetStatus(pspResult.Data.Status);
            donation.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            donation.SetExecutedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);
            return Success();
        }

        private async Task<Result> HandleWithholdingStatusAsync(RefreshTransferStatusCommand request,
            Domain.Withholding withholding, CancellationToken token)
        {
            if (withholding.Status == TransactionStatus.Succeeded || withholding.Status == TransactionStatus.Failed)
                return Success();

            var pspResult = await _pspService.GetTransferAsync(withholding.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult);

            withholding.SetStatus(pspResult.Data.Status);
            withholding.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            withholding.SetExecutedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}