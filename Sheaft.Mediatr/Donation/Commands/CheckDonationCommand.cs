using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Donation.Commands
{
    public class CheckDonationCommand : Command
    {
        protected CheckDonationCommand()
        {
            
        }
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
            var donation = await _context.Donations.SingleAsync(e => e.Id == request.DonationId, token);
            if (donation.Status != TransactionStatus.Created && donation.Status != TransactionStatus.Waiting)
                return Failure(MessageKind.BadRequest);

            var result =
                await _mediatr.Process(new RefreshDonationStatusCommand(request.RequestUser, donation.Identifier),
                    token);
            if (!result.Succeeded)
                return Failure(result);

            return Success();
        }
    }
}