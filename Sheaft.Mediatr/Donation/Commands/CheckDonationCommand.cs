using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Donation.Commands
{
    public class CheckDonationCommand : Command
    {
        [JsonConstructor]
        public CheckDonationCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid DonationId { get; set; }
    }

    public class CheckDonationCommandHandler : CommandsHandler,
        IRequestHandler<CheckDonationCommand, Result>
    {
        public CheckDonationCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CheckDonationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckDonationCommand request, CancellationToken token)
        {
            var donation = await _context.GetByIdAsync<Domain.Donation>(request.DonationId, token);
            if (donation.Status != TransactionStatus.Created && donation.Status != TransactionStatus.Waiting)
                return Failure();

            var result =
                await _mediatr.Process(new RefreshDonationStatusCommand(request.RequestUser, donation.Identifier),
                    token);
            if (!result.Succeeded)
                return Failure(result.Exception);

            return Success();
        }
    }
}