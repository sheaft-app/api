using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckDonationsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckDonationsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }

    public class CheckDonationsCommandHandler : CommandsHandler,
        IRequestHandler<CheckDonationsCommand, Result<bool>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;
        private readonly PspOptions _pspOptions;

        public CheckDonationsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CheckDonationsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckDonationsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var skip = 0;
                const int take = 100;

                var donationIds = await GetNextDonationIdsAsync(skip, take, token);
                while (donationIds.Any())
                {
                    foreach (var donationId in donationIds)
                    {
                        _mediatr.Post(new CheckDonationCommand(request.RequestUser)
                        {
                            DonationId = donationId
                        });
                    }

                    skip += take;
                    donationIds = await GetNextDonationIdsAsync(skip, take, token);
                }

                return Ok(true);
            });
        }

        private async Task<IEnumerable<Guid>> GetNextDonationIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Donations
                .Get(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created, true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
