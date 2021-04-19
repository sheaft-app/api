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
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Transfer.Commands
{
    public class CheckTransferCommand : Command
    {
        [JsonConstructor]
        public CheckTransferCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransferId { get; set; }
    }

    public class CheckTransferCommandHandler : CommandsHandler,
        IRequestHandler<CheckTransferCommand, Result>
    {
        public CheckTransferCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckTransferCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckTransferCommand request, CancellationToken token)
        {
            var transfer = await _context.Transfers.SingleAsync(e => e.Id == request.TransferId, token);
            if (transfer.Status == TransactionStatus.Created || transfer.Status == TransactionStatus.Waiting)
            {
                var result =
                    await _mediatr.Process(new RefreshTransferStatusCommand(request.RequestUser, transfer.Identifier),
                        token);
                if (!result.Succeeded)
                    return Failure(result);
            }

            return Success();
        }
    }
}