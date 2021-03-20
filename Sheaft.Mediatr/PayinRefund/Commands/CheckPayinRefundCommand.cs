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

namespace Sheaft.Mediatr.PayinRefund.Commands
{
    public class CheckPayinRefundCommand : Command
    {
        [JsonConstructor]
        public CheckPayinRefundCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid PayinRefundId { get; set; }
    }

    public class CheckPayinRefundCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayinRefundCommand, Result>
    {
        public CheckPayinRefundCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<CheckPayinRefundCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CheckPayinRefundCommand request, CancellationToken token)
        {
            var payinRefund = await _context.GetByIdAsync<Domain.PayinRefund>(request.PayinRefundId, token);
            if (payinRefund.Status != TransactionStatus.Created && payinRefund.Status != TransactionStatus.Waiting)
                return Failure();

            var result =
                await _mediatr.Process(
                    new RefreshPayinRefundStatusCommand(request.RequestUser, payinRefund.Identifier), token);
            if (!result.Succeeded)
                return Failure(result.Exception);

            return Success();
        }
    }
}