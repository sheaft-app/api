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
    public class RefreshPayinStatusCommand : Command<TransactionStatus>
    {
        [JsonConstructor]
        public RefreshPayinStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
    
    public class RefreshPayinStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshPayinStatusCommand, Result<TransactionStatus>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public RefreshPayinStatusCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<RefreshPayinStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<TransactionStatus>> Handle(RefreshPayinStatusCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payin = await _context.GetSingleAsync<Payin>(c => c.Identifier == request.Identifier, token);
                if (payin.Status == TransactionStatus.Succeeded || payin.Status == TransactionStatus.Failed)
                    return Ok(payin.Status);

                var pspResult = await _pspService.GetPayinAsync(payin.Identifier, token);
                if (!pspResult.Success)
                    return Failed<TransactionStatus>(pspResult.Exception);

                payin.SetStatus(pspResult.Data.Status);
                payin.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
                payin.SetExecutedOn(pspResult.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);

                switch (payin.Status)
                {
                    case TransactionStatus.Failed:
                        _mediatr.Post(new FailOrderCommand(request.RequestUser) { OrderId = payin.Order.Id, PayinId = payin.Id });
                        break;
                    case TransactionStatus.Succeeded:
                        _mediatr.Post(new ConfirmOrderCommand(request.RequestUser) { OrderId = payin.Order.Id });
                        _mediatr.Post(new PayinSucceededEvent(request.RequestUser) { PayinId = payin.Id });
                        break;
                }

                return Ok(payin.Status);
            });
        }
    }
}
