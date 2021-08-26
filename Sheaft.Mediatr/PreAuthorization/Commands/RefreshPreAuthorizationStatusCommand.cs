using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.Mediatr.PreAuthorization.Commands
{
    public class RefreshPreAuthorizationStatusCommand : Command<PreAuthorizationStatus>
    {
        protected RefreshPreAuthorizationStatusCommand()
        {
            
        }
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
            var preAuthorization = await _context.PreAuthorizations.
                SingleOrDefaultAsync(c => c.Identifier == request.Identifier, token);
            
            if (preAuthorization.Processed)
                return Success(preAuthorization.Status);

            var pspResult = await _pspService.GetPreAuthorizationAsync(preAuthorization.Identifier, token);
            if (!pspResult.Succeeded)
                return Failure<PreAuthorizationStatus>(pspResult);

            preAuthorization.SetStatus(pspResult.Data.Status);
            preAuthorization.SetPaymentStatus(pspResult.Data.PaymentStatus);
            preAuthorization.SetExpirationDate(pspResult.Data.ExpirationDate);
            preAuthorization.SetResult(pspResult.Data.ResultCode, pspResult.Data.ResultMessage);

            if(preAuthorization.PaymentStatus != PaymentStatus.Waiting)
                preAuthorization.SetAsProcessed();
            
            await _context.SaveChangesAsync(token);

            var order = await _context.Orders.SingleAsync(o => o.Id == preAuthorization.OrderId, token);
            if(order.Status != OrderStatus.Waiting && order.Status != OrderStatus.Validated && order.Status != OrderStatus.Created)
                return Success(preAuthorization.Status);
            
            switch (preAuthorization.Status)
            {
                case PreAuthorizationStatus.Cancelled:
                case PreAuthorizationStatus.Failed:
                    _mediatr.Post(new FailOrderCommand(request.RequestUser)
                        {OrderId = preAuthorization.OrderId});
                    break;
                case PreAuthorizationStatus.Succeeded:
                    _mediatr.Post(new ConfirmOrderCommand(request.RequestUser) {OrderId = preAuthorization.OrderId});
                    break;
            }

            return Success(preAuthorization.Status);
        }
    }
}