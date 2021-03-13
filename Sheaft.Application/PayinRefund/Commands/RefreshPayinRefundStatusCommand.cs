using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.PayinRefund;

namespace Sheaft.Application.PayinRefund.Commands
{
    public class RefreshPayinRefundStatusCommand : Command
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
        IRequestHandler<RefreshPayinRefundStatusCommand, Result>
    {
        private readonly IPspService _pspService;

        public RefreshPayinRefundStatusCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<RefreshPayinRefundStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result> Handle(RefreshPayinRefundStatusCommand request,
            CancellationToken token)
        {
            var payinRefund =
                await _context.GetSingleAsync<Domain.PayinRefund>(c => c.Identifier == request.Identifier, token);
            if (payinRefund.Status == TransactionStatus.Succeeded || payinRefund.Status == TransactionStatus.Failed)
                return Success();

            var pspResult = await _pspService.GetRefundAsync(payinRefund.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure(pspResult.Exception);

            payinRefund.SetStatus(pspResult.Data.Status);
            payinRefund.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);
            payinRefund.SetExecutedOn(pspResult.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}