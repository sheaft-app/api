using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckDonationCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckDonationCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid DonationId { get; set; }
    }

    public class CheckDonationCommandHandler : CommandsHandler,
        IRequestHandler<CheckDonationCommand, Result<bool>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;
        private readonly PspOptions _pspOptions;

        public CheckDonationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CheckDonationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckDonationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var donation = await _context.GetByIdAsync<Donation>(request.DonationId, token);
                if (donation.Status != TransactionStatus.Created && donation.Status != TransactionStatus.Waiting)
                    return Ok(false);

                var result =
                    await _mediatr.Process(new RefreshDonationStatusCommand(request.RequestUser, donation.Identifier),
                        token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }
    }
}
