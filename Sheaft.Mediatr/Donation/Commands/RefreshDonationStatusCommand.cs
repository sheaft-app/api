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

namespace Sheaft.Mediatr.Donation.Commands
{
    public class RefreshDonationStatusCommand : Command
    {
        [JsonConstructor]
        public RefreshDonationStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshDonationStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshDonationStatusCommand, Result>
    {
        private readonly IPspService _pspService;

        public RefreshDonationStatusCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<RefreshDonationStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(RefreshDonationStatusCommand request,
            CancellationToken token)
        {
            var donation = await _context.Donations
                .SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);
            if (donation.Status == TransactionStatus.Succeeded || donation.Status == TransactionStatus.Failed)
                return Failure(MessageKind.BadRequest);

            var pspResult = await _pspService.GetTransferAsync(donation.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult);

            donation.SetStatus(pspResult.Data.Status);
            donation.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            donation.SetExecutedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}