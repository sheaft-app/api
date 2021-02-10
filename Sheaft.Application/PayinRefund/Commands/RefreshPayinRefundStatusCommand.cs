using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class RefreshPayinRefundStatusCommand : Command<TransactionStatus>
    {
        [JsonConstructor]
        public RefreshPayinRefundStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshPayinRefundStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshPayinRefundStatusCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public RefreshPayinRefundStatusCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<RefreshPayinRefundStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayinRefundStatusCommand request,
            CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payinRefund =
                    await _context.GetSingleAsync<PayinRefund>(c => c.Identifier == request.Identifier, token);
                if (payinRefund.Status == TransactionStatus.Succeeded || payinRefund.Status == TransactionStatus.Failed)
                    return Ok(payinRefund.Status);

                var pspResult = await _pspService.GetRefundAsync(payinRefund.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                payinRefund.SetStatus(pspResult.Data.Status);
                payinRefund.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                payinRefund.SetExecutedOn(pspResult.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);

                switch (payinRefund.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new PayinRefundFailedEvent(request.RequestUser) {RefundId = payinRefund.Id});
                        break;
                }

                return Ok(payinRefund.Status);
            });
        }
    }
}
