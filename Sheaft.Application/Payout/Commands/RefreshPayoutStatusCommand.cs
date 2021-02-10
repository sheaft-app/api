using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class RefreshPayoutStatusCommand : Command<TransactionStatus>
    {
        [JsonConstructor]
        public RefreshPayoutStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
    public class RefreshPayoutStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshPayoutStatusCommand, Result<TransactionStatus>>
    {
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;
        private readonly IPspService _pspService;

        public RefreshPayoutStatusCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IPspService pspService,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<RefreshPayoutStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayoutStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payout = await _context.GetSingleAsync<Payout>(c => c.Identifier == request.Identifier, token);
                if (payout.Status == TransactionStatus.Succeeded || payout.Status == TransactionStatus.Failed)
                    return Ok(payout.Status);

                var pspResult = await _pspService.GetPayoutAsync(payout.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                payout.SetStatus(pspResult.Data.Status);
                payout.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                payout.SetExecutedOn(pspResult.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);

                switch (payout.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new PayoutFailedEvent(request.RequestUser) { PayoutId = payout.Id });
                        break;
                    case TransactionStatus.Succeeded:
                        if(payout.Withholdings.Any())
                            _mediatr.Post(new ProcessWithholdingsCommand(request.RequestUser) { PayoutId = payout.Id });
                        break;
                }

                return Ok(payout.Status);
            });
        }
    }
}
