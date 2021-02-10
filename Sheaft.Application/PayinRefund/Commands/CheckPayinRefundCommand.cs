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

namespace Sheaft.Application.PayinRefund.Commands
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
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public CheckPayinRefundCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CheckPayinRefundCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
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