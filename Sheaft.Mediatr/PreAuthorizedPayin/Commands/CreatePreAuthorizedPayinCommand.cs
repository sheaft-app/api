using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.PreAuthorizedPayin
{
    public class CreatePreAuthorizedPayinCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePreAuthorizedPayinCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid PreAuthorizationId { get; set; }
        public Guid PurchaseOrderId { get; set; }
    }
    
    public class CreatePreAuthorizedPayinCommandHandler : CommandsHandler,
        IRequestHandler<CreatePreAuthorizedPayinCommand, Result<Guid>>
    {
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;

        public CreatePreAuthorizedPayinCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IPspService pspService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreatePreAuthorizedPayinCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreatePreAuthorizedPayinCommand request, CancellationToken token)
        {
            var preAuthorization =
                await _context.GetByIdAsync<Domain.PreAuthorization>(request.PreAuthorizationId, token);
            if (preAuthorization.Order.Status == OrderStatus.Validated)
                return Failure<Guid>(MessageKind.Payin_CannotCreate_Order_Already_Validated);

            var wallet = await _context.GetSingleAsync<Domain.Wallet>(c => c.User.Id == request.RequestUser.Id, token);
            if (preAuthorization.Order.TotalOnSalePrice < 1)
                return Failure<Guid>(MessageKind.Order_Total_CannotBe_LowerThan, 1);

            var preAuthorizedPayin =
                new Domain.PreAuthorizedPayin(Guid.NewGuid(), preAuthorization, wallet);

            await _context.AddAsync(preAuthorizedPayin, token);
            await _context.SaveChangesAsync(token);

            var result = await _pspService.CreatePreAuthorizedPayinAsync(preAuthorizedPayin, token);
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