using System;
using System.Linq;
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

namespace Sheaft.Mediatr.Payin.Commands
{
    public class CreatePreAuthorizedPayinCommand : Command<Guid>
    {
        protected CreatePreAuthorizedPayinCommand()
        {
        }

        [JsonConstructor]
        public CreatePreAuthorizedPayinCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PreAuthorizationId { get; set; }
    }

    public class CreatePreAuthorizedPayinCommandHandler : CommandsHandler,
        IRequestHandler<CreatePreAuthorizedPayinCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public CreatePreAuthorizedPayinCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IPspService pspService,
            ILogger<CreatePreAuthorizedPayinCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreatePreAuthorizedPayinCommand request, CancellationToken token)
        {
            var preAuthorization =
                await _context.PreAuthorizations.SingleAsync(e => e.Id == request.PreAuthorizationId, token);
            if (preAuthorization.Status != PreAuthorizationStatus.Succeeded
                && preAuthorization.Order.Status != OrderStatus.Confirmed
                && !preAuthorization.Order.PurchaseOrders.Any(po =>
                    po.AcceptedOn.HasValue
                    && !po.DroppedOn.HasValue))
                return Success<Guid>();

            if (preAuthorization.PreAuthorizedPayin != null &&
                (preAuthorization.PreAuthorizedPayin.Status == TransactionStatus.Waiting ||
                 preAuthorization.PreAuthorizedPayin.Status == TransactionStatus.Succeeded))
                return Failure<Guid>("Un virement est déjà en attente pour ce pré-paiement.");

            if (preAuthorization.ExpirationDate < DateTimeOffset.UtcNow)
                return Failure<Guid>("Le pré-paiement a expiré.");

            var wallet =
                await _context.Wallets.SingleOrDefaultAsync(c => c.UserId == preAuthorization.Order.UserId, token);
            if (preAuthorization.Order.TotalOnSalePrice < 1)
                return Failure<Guid>("Le montant du virement doit être supérieur à 1€.");

            var preAuthorizedPayin =
                new Domain.PreAuthorizedPayin(Guid.NewGuid(), preAuthorization, wallet);

            preAuthorization.SetPreAuthorizedPayin(preAuthorizedPayin);

            await _context.AddAsync(preAuthorizedPayin, token);
            await _context.SaveChangesAsync(token);

            var result = await _pspService.CreatePreAuthorizedPayinAsync(preAuthorization, token);
            if (!result.Succeeded)
                return Failure<Guid>(result);

            preAuthorizedPayin.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
            preAuthorizedPayin.SetIdentifier(result.Data.Identifier);
            preAuthorizedPayin.SetStatus(result.Data.Status);
            preAuthorizedPayin.SetCreditedAmount(result.Data.Credited);
            preAuthorizedPayin.SetExecutedOn(result.Data.ProcessedOn);

            await _context.SaveChangesAsync(token);
            return Success(preAuthorizedPayin.Id);
        }
    }
}