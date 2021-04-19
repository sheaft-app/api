using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Donation.Commands
{
    public class CheckDonationsCommand : Command
    {
        protected CheckDonationsCommand()
        {
            
        }
        [JsonConstructor]
        public CheckDonationsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }

    public class CheckDonationsCommandHandler : CommandsHandler,
        IRequestHandler<CheckDonationsCommand, Result>
    {
        public CheckDonationsCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CheckDonationsCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckDonationsCommand request, CancellationToken token)
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

            return Success();
        }

        private async Task<IEnumerable<Guid>> GetNextDonationIdsAsync(int skip, int take, CancellationToken token)
        {
            return await _context.Donations
                .Where(c => c.Status == TransactionStatus.Waiting || c.Status == TransactionStatus.Created)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}