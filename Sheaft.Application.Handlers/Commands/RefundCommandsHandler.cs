using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Handlers
{
    public class RefundCommandsHandler : ResultsHandler,
        IRequestHandler<RefreshPayinRefundStatusCommand, Result<TransactionStatus>>,
        IRequestHandler<RefreshTransferRefundStatusCommand, Result<TransactionStatus>>,
        IRequestHandler<RefreshPayoutRefundStatusCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;

        public RefundCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<TransferCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public Task<Result<TransactionStatus>> Handle(RefreshPayinRefundStatusCommand request, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TransactionStatus>> Handle(RefreshTransferRefundStatusCommand request, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TransactionStatus>> Handle(RefreshPayoutRefundStatusCommand request, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
