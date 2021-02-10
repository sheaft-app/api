using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckPayinRefundCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckPayinRefundCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid PayinRefundId { get; set; }
    }

    public class CheckPayinRefundCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayinRefundCommand, Result<bool>>
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
        
        public async Task<Result<bool>> Handle(CheckPayinRefundCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payinRefund = await _context.GetByIdAsync<PayinRefund>(request.PayinRefundId, token);
                if (payinRefund.Status != TransactionStatus.Created && payinRefund.Status != TransactionStatus.Waiting)
                    return Ok(false);

                var result =
                    await _mediatr.Process(
                        new RefreshPayinRefundStatusCommand(request.RequestUser, payinRefund.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }
    }
}
