using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CheckPayoutCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckPayoutCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayoutId { get; set; }
    }
    
    public class CheckPayoutCommandHandler : CommandsHandler,
        IRequestHandler<CheckPayoutCommand, Result<bool>>
    {
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;
        private readonly IPspService _pspService;

        public CheckPayoutCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CheckPayoutCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<bool>> Handle(CheckPayoutCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payout = await _context.GetByIdAsync<Payout>(request.PayoutId, token);
                if (payout.Status != TransactionStatus.Created && payout.Status != TransactionStatus.Waiting)
                    return Ok(false);

                var result = await _mediatr.Process(new RefreshPayoutStatusCommand(request.RequestUser, payout.Identifier), token);
                if (!result.Success)
                    return Failed<bool>(result.Exception);

                return Ok(true);
            });
        }
    }
}
