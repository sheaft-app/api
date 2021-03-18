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

namespace Sheaft.Services.Transfer.Commands
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
            var transfer = await _context.GetByIdAsync<Domain.Transfer>(request.TransferId, token);
            if (transfer.Status != TransactionStatus.Created && transfer.Status != TransactionStatus.Waiting)
                return Failure();

            var result =
                await _mediatr.Process(new RefreshTransferStatusCommand(request.RequestUser, transfer.Identifier),
                    token);
            if (!result.Succeeded)
                return Failure(result.Exception);

            return Success();
        }
    }
}