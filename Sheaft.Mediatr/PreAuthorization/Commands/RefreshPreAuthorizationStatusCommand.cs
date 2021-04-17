using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.Mediatr.PreAuthorization.Commands
{
    public class RefreshPreAuthorizationStatusCommand : Command<PreAuthorizationStatus>
    {
        [JsonConstructor]
        public RefreshPreAuthorizationStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }

    public class RefreshPreAuthorizationStatusCommandHandler : CommandsHandler,
        IRequestHandler<RefreshPreAuthorizationStatusCommand, Result<PreAuthorizationStatus>>
    {
        private readonly IPspService _pspService;

        public RefreshPreAuthorizationStatusCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IPspService pspService,
            ILogger<RefreshPreAuthorizationStatusCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<PreAuthorizationStatus>> Handle(RefreshPreAuthorizationStatusCommand request,
            CancellationToken token)
        {
            var preAuthorization =
                await _context.GetSingleAsync<Domain.PreAuthorization>(c => c.Identifier == request.Identifier, token);
            if ((preAuthorization.Status == PreAuthorizationStatus.Succeeded && preAuthorization.PaymentStatus == PaymentStatus.Validated) ||
                preAuthorization.Status == PreAuthorizationStatus.Failed || 
                preAuthorization.Status == PreAuthorizationStatus.Cancelled)
                return Success(preAuthorization.Status);

            var pspResult = await _pspService.GetPreAuthorizationAsync(preAuthorization.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure<PreAuthorizationStatus>(pspResult.Exception);

            preAuthorization.SetStatus(pspResult.Data.Status);
            preAuthorization.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);

            await _context.SaveChangesAsync(token);

            switch (preAuthorization.Status)
            {
                case PreAuthorizationStatus.Failed:
                    _mediatr.Post(new FailOrderCommand(request.RequestUser)
                        {OrderId = preAuthorization.Order.Id});
                    break;
                case PreAuthorizationStatus.Succeeded:
                    _mediatr.Post(new ConfirmOrderCommand(request.RequestUser) {OrderId = preAuthorization.Order.Id});
                    break;
            }

            return Success(preAuthorization.Status);
        }
    }
}