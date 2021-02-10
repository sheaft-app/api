using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Payin.Commands
{
    public class CheckPayinCommand : Command
    {
        [JsonConstructor]
        public CheckPayinCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayinId { get; set; }
    }

    public class CheckPayinCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayinCommand, Result>
    {
        public CheckPayinCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckPayinCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckPayinCommand request, CancellationToken token)
        {
            var payin = await _context.GetByIdAsync<Domain.Payin>(request.PayinId, token);
            if (payin.Status != TransactionStatus.Created && payin.Status != TransactionStatus.Waiting)
                return Failure();

            var result = await _mediatr.Process(new RefreshPayinStatusCommand(request.RequestUser, payin.Identifier),
                token);
            if (!result.Succeeded)
                return Failure(result.Exception);

            return Success();
        }
    }
}