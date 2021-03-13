using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Payout.Commands
{
    public class CheckPayoutCommand : Command
    {
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
            var payout = await _context.GetByIdAsync<Domain.Payout>(request.PayoutId, token);
            if (payout.Status != TransactionStatus.Created && payout.Status != TransactionStatus.Waiting)
                return Failure();

            var result = await _mediatr.Process(new RefreshPayoutStatusCommand(request.RequestUser, payout.Identifier),
                token);
            if (!result.Succeeded)
                return Failure(result.Exception);

            return Success();
        }
    }
}