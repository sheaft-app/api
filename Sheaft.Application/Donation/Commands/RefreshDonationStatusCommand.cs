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
using Sheaft.Domain.Events.Donation;

namespace Sheaft.Application.Donation.Commands
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
            var donation =
                await _context.GetSingleAsync<Domain.Donation>(c => c.Identifier == request.Identifier, token);
            if (donation.Status == TransactionStatus.Succeeded || donation.Status == TransactionStatus.Failed)
                return Failure();

            var pspResult = await _pspService.GetTransferAsync(donation.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult.Exception);

            donation.SetStatus(pspResult.Data.Status);
            donation.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            donation.SetExecutedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}