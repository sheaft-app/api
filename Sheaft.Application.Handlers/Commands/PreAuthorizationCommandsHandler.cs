using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using System.Linq;
using Sheaft.Domain.Enums;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Sheaft.Exceptions;

namespace Sheaft.Application.Handlers
{
    public class PreAuthorizationCommandsHandler : ResultsHandler,
        IRequestHandler<CreatePreAuthorizationCommand, Result<Guid>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly PspOptions _pspOptions;
        private readonly IPspService _pspService;

        public PreAuthorizationCommandsHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IPspService pspService,
            ICapingDeliveriesService capingDeliveriesService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<PreAuthorizationCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _capingDeliveriesService = capingDeliveriesService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreatePreAuthorizationCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);

                var checkResult = await _mediatr.Process(new CheckConsumerConfigurationCommand(request.RequestUser) { Id = order.User.Id }, token);
                if (!checkResult.Success)
                    return Failed<Guid>(checkResult.Exception);

                if (!order.Deliveries.Any())
                    return BadRequest<Guid>(MessageKind.Order_CannotPay_Deliveries_Required);

                var validatedDeliveries = await _capingDeliveriesService.ValidateCapedDeliveriesAsync(order.Deliveries, token);
                if (!validatedDeliveries.Success)
                    return Failed<Guid>(validatedDeliveries.Exception);

                var products = await _context.GetByIdsAsync<Product>(order.Products.Select(p => p.Id), token);

                var invalidProductIds = products.Where(p => p.RemovedOn.HasValue || !p.Available || !p.VisibleToConsumers || p.Producer.RemovedOn.HasValue || !p.Producer.CanDirectSell).Select(p => p.Id.ToString("N"));
                if (invalidProductIds.Any())
                    return BadRequest<Guid>(MessageKind.Order_CannotPay_Some_Products_Invalid, string.Join(";", invalidProductIds));

                var card = await _context.GetSingleAsync<Card>(c => c.Identifier == request.CardIdentifier, token);
                if(card == null)
                {
                    card = new Card(Guid.NewGuid(), request.CardIdentifier, $"Carte_{DateTime.UtcNow.ToString("YYYYMMDDTHHmmss")}", order.User);
                    await _context.AddAsync(card, token);
                    await _context.SaveChangesAsync(token);
                }

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var preAuthorization = new PreAuthorization(Guid.NewGuid(), order, card, _pspOptions.PreAuthorizeUrl);
                    await _context.AddAsync(preAuthorization, token);
                    await _context.SaveChangesAsync(token);

                    var referenceResult = await _mediatr.Process(new CreateOrderIdentifierCommand(request.RequestUser), token);
                    if (!referenceResult.Success)
                        return Failed<Guid>(referenceResult.Exception);

                    order.SetReference(referenceResult.Data);
                    order.SetStatus(OrderStatus.Waiting);
                    await _context.SaveChangesAsync(token);

                    var result = await _pspService.CreatePreAuthorizationAsync(preAuthorization, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

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

                    return Ok(preAuthorization.Id);
                } 
            });
        }
    }
}
