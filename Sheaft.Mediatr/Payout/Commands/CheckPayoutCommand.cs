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

namespace Sheaft.Mediatr.Payout.Commands
{
    public class CheckPayoutCommand : Command
    {
        protected CheckPayoutCommand()
        {
            
        }
        [JsonConstructor]
        public CheckPayoutCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayoutId { get; set; }
    }

    public class CheckPayoutCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayoutCommand, Result>
    {
        public CheckPayoutCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CheckPayoutCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckPayoutCommand request, CancellationToken token)
        {
            var payout = await _context.Payouts.SingleAsync(e => e.Id == request.PayoutId, token);
            if (payout.Status == TransactionStatus.Created || payout.Status == TransactionStatus.Waiting)
            {
                var result = await _mediatr.Process(
                    new RefreshPayoutStatusCommand(request.RequestUser, payout.Identifier),
                    token);
                if (!result.Succeeded)
                    return Failure(result);
            }

            return Success();
        }
    }
}