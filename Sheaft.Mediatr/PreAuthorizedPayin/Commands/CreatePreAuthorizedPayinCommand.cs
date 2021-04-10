using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.PreAuthorizedPayin
{
    public class CreatePreAuthorizedPayinCommand : Command<Guid>
    {
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
            var preAuthorization = await _context.GetByIdAsync<Domain.PreAuthorization>(request.PreAuthorizationId, token);
            if (preAuthorization.Status != PreAuthorizationStatus.Succeeded 
                && preAuthorization.PaymentStatus != PaymentStatus.Validated 
                && preAuthorization.Order.Status != OrderStatus.Validated 
                && !preAuthorization.Order.PurchaseOrders.Any(po =>
                    po.AcceptedOn.HasValue 
                    && !po.WithdrawnOn.HasValue 
                    && po.CreatedOn.AddDays(5) < DateTimeOffset.UtcNow 
                    && po.CreatedOn.AddDays(7) > DateTimeOffset.UtcNow)
                && preAuthorization.PreAuthorizedPayin != null)
                return Failure<Guid>(MessageKind.Validation);

            var wallet = await _context.GetSingleAsync<Domain.Wallet>(c => c.User.Id == request.RequestUser.Id, token);
            if (preAuthorization.Order.TotalOnSalePrice < 1)
                return Failure<Guid>(MessageKind.Order_Total_CannotBe_LowerThan, 1);

            var preAuthorizedPayin =
                new Domain.PreAuthorizedPayin(Guid.NewGuid(), preAuthorization, wallet);

            preAuthorization.SetPreAuthorizedPayin(preAuthorizedPayin);
            
            await _context.AddAsync(preAuthorizedPayin, token);
            await _context.SaveChangesAsync(token);

            var result = await _pspService.CreatePreAuthorizedPayinAsync(preAuthorization, token);
            if (!result.Succeeded)
                return Failure<Guid>(result.Exception);

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