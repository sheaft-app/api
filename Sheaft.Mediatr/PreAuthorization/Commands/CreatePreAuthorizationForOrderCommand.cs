using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Agreement;
using Sheaft.Mediatr.Consumer.Commands;
using Sheaft.Mediatr.Order.Commands;
using Sheaft.Options;

namespace Sheaft.Mediatr.PreAuthorization
{
    public class CreatePreAuthorizationForOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePreAuthorizationForOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid OrderId { get; set; }
        public string CardIdentifier { get; set; }
    }
    
    public class CreatePreAuthorizationCommandHandler : CommandsHandler,
        IRequestHandler<CreatePreAuthorizationForOrderCommand, Result<Guid>>
    {
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;
        private readonly IOrderService _orderService;

        public CreatePreAuthorizationCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IPspService pspService,
            IOrderService orderService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreatePreAuthorizationCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _orderService = orderService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreatePreAuthorizationForOrderCommand request, CancellationToken token)
        {
            var validationResult = await _orderService.ValidateConsumerOrderAsync(request.OrderId, request.RequestUser, token);
            if (!validationResult.Succeeded)
                return Failure<Guid>(validationResult.Message);

            var checkResult = await _mediatr.Process(new CheckConsumerConfigurationCommand(request.RequestUser), token);
            if (!checkResult.Succeeded)
                return Failure<Guid>(checkResult.Exception);

            var order = await _context.GetByIdAsync<Domain.Order>(request.OrderId, token);
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var card = await _context.FindSingleAsync<Card>(c => c.Identifier == request.CardIdentifier, token);
                if (card == null)
                {
                    card = new Card(Guid.NewGuid(), request.CardIdentifier,
                        $"Carte_{DateTime.UtcNow.ToString("YYYYMMDDTHHmmss")}", order.User);
                    await _context.AddAsync(card, token);
                    await _context.SaveChangesAsync(token);
                }

                var preAuthorization =
                    new Domain.PreAuthorization(Guid.NewGuid(), order, card, _pspOptions.PreAuthorizeUrl);
                await _context.AddAsync(preAuthorization, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreatePreAuthorizationAsync(preAuthorization, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result.Exception);

                preAuthorization.SetIdentifier(result.Data.Identifier);
                preAuthorization.SetStatus(result.Data.Status);
                preAuthorization.SetPaymentStatus(result.Data.PaymentStatus);
                preAuthorization.SetExpirationDate(result.Data.ExpirationDate);
                preAuthorization.SetRemaining(result.Data.Remaining);
                preAuthorization.SetDebited(result.Data.Debited);
                preAuthorization.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                preAuthorization.SetSecureModeRedirectUrl(result.Data.SecureModeRedirectUrl);
                
                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);
                
                return Success(preAuthorization.Id);
            }
        }
    }
}