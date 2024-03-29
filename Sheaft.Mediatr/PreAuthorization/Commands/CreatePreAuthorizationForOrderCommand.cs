using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Consumer.Commands;

namespace Sheaft.Mediatr.PreAuthorization.Commands
{
    public class CreatePreAuthorizationForOrderCommand : Command<Guid>
    {
        protected CreatePreAuthorizationForOrderCommand()
        {
            
        }
        [JsonConstructor]
        public CreatePreAuthorizationForOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid OrderId { get; set; }
        public string CardIdentifier { get; set; }
        public string IpAddress { get; set; }
        public BrowserInfoDto BrowserInfo { get; set; }
    }
    
    public class CreatePreAuthorizationForOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreatePreAuthorizationForOrderCommand, Result<Guid>>
    {
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;
        private readonly IOrderService _orderService;

        public CreatePreAuthorizationForOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IPspService pspService,
            IOrderService orderService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreatePreAuthorizationForOrderCommandHandler> logger)
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
                return Failure<Guid>(validationResult);

            var checkResult = await _mediatr.Process(new CheckConsumerConfigurationCommand(request.RequestUser), token);
            if (!checkResult.Succeeded)
                return Failure<Guid>(checkResult);

            var order = await _context.Orders.SingleAsync(e => e.Id == request.OrderId, token);
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var card = await _context.Cards.SingleOrDefaultAsync(c => c.Identifier == request.CardIdentifier, token);
                if (card == null)
                {
                    card = new Domain.Card(Guid.NewGuid(), request.CardIdentifier,
                        $"Carte_{DateTime.UtcNow.ToString("YYYYMMDDTHHmmss")}", order.User);
                    await _context.AddAsync(card, token);
                    await _context.SaveChangesAsync(token);
                }

                var preAuthorization =
                    new Domain.PreAuthorization(Guid.NewGuid(), order, card, _pspOptions.PreAuthorizeUrl);
                await _context.AddAsync(preAuthorization, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreatePreAuthorizationAsync(preAuthorization, request.IpAddress, request.BrowserInfo, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result);

                preAuthorization.SetIdentifier(result.Data.Identifier);
                preAuthorization.SetStatus(result.Data.Status);
                preAuthorization.SetPaymentStatus(result.Data.PaymentStatus);
                preAuthorization.SetExpirationDate(result.Data.ExpirationDate);
                preAuthorization.SetRemaining(result.Data.Remaining);
                preAuthorization.SetDebited(result.Data.Debited);
                preAuthorization.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                preAuthorization.SetSecureModeRedirectUrl(result.Data.SecureModeRedirectUrl);
                preAuthorization.SetSecureModeNeeded(result.Data.SecureModeNeeded);
                
                order.SetStatus(OrderStatus.Validated);
                
                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);
                
                return Success(preAuthorization.Id);
            }
        }
    }
}