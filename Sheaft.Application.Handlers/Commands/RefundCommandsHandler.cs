using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using Sheaft.Domain.Enums;
using Sheaft.Application.Events;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class RefundCommandsHandler : ResultsHandler,
        IRequestHandler<RefreshPayinRefundStatusCommand, Result<bool>>,
        IRequestHandler<RefreshTransferRefundStatusCommand, Result<bool>>,
        IRequestHandler<RefreshPayoutRefundStatusCommand, Result<bool>>
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

        public Task<Result<bool>> Handle(RefreshPayinRefundStatusCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(RefreshTransferRefundStatusCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(RefreshPayoutRefundStatusCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
