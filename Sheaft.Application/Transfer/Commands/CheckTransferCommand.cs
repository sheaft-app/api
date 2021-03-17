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

namespace Sheaft.Application.Transfer.Commands
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