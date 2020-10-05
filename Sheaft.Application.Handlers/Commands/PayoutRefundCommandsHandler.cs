using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Enums;
using Sheaft.Application.Events;

namespace Sheaft.Application.Handlers
{
    public class PayoutRefundCommandsHandler : ResultsHandler,
        IRequestHandler<RefreshPayoutRefundStatusCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;

        public PayoutRefundCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<PayoutRefundCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayoutRefundStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var pspResult = await _pspService.GetRefundAsync(request.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                switch (pspResult.Data.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new PayoutRefundFailedEvent(request.RequestUser) { RefundIdentifier = request.Identifier });
                        break;
                }

                return Ok(pspResult.Data.Status);
            });
        }
    }
}
