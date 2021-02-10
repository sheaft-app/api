using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class RefreshDonationStatusCommand : Command<TransactionStatus>
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
        IRequestHandler<RefreshDonationStatusCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;
        private readonly PspOptions _pspOptions;

        public RefreshDonationStatusCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<RefreshDonationStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
            _pspOptions = pspOptions.Value;
        }
        public async Task<Result<TransactionStatus>> Handle(RefreshDonationStatusCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var donation = await _context.GetSingleAsync<Donation>(c => c.Identifier == request.Identifier, token);
                if (donation.Status == TransactionStatus.Succeeded || donation.Status == TransactionStatus.Failed)
                    return Ok(donation.Status);

                var pspResult = await _pspService.GetTransferAsync(donation.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                donation.SetStatus(pspResult.Data.Status);
                donation.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                donation.SetExecutedOn(pspResult.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);

                switch (donation.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new DonationFailedEvent(request.RequestUser) {DonationId = donation.Id});
                        break;
                }

                return Ok(donation.Status);
            });
        }
    }
}
