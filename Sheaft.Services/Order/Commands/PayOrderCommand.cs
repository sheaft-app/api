using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Services.Consumer.Commands;
using Sheaft.Services.Payin.Commands;

namespace Sheaft.Services.Order.Commands
{
    public class PayOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public PayOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }

    public class PayOrderCommandHandler : CommandsHandler,
        IRequestHandler<PayOrderCommand, Result<Guid>>
    {
        private readonly IIdentifierService _identifierService;
        private readonly IDeliveryService _deliveryService;

        public PayOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IIdentifierService identifierService,
            IDeliveryService deliveryService,
            ILogger<PayOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
            _deliveryService = deliveryService;
        }

        public async Task<Result<Guid>> Handle(PayOrderCommand request, CancellationToken token)
        {
            var order = await _context.GetByIdAsync<Domain.Order>(request.OrderId, token);
            if(order.User == null)
                throw SheaftException.BadRequest(MessageKind.Order_CannotPay_User_Required);
            
            if(order.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            if (!order.Deliveries.Any())
                return Failure<Guid>(MessageKind.Order_CannotPay_Deliveries_Required);

            foreach (var delivery in order.Deliveries)
            {
                if(delivery.DeliveryMode.Closings.Any(c => DateTimeOffset.Now >= c.ClosedFrom && DateTimeOffset.UtcNow <= c.ClosedTo))
                    return Failure<Guid>(MessageKind.Order_CannotCreate_Delivery_Closed, delivery.DeliveryMode.Name);
            }
            
            var checkResult =
                await _mediatr.Process(
                    new CheckConsumerConfigurationCommand(request.RequestUser) {ConsumerId = order.User.Id}, token);
            if (!checkResult.Succeeded)
                return Failure<Guid>(checkResult.Exception);

            var validatedDeliveries = await _deliveryService.ValidateCapedDeliveriesAsync(order.Deliveries, token);
            if (!validatedDeliveries.Succeeded)
                return Failure<Guid>(validatedDeliveries.Exception);

            var products = await _context.Products.Where(p => order.Products.Select(p => p.Id).Contains(p.Id)).ToListAsync(token);
            var invalidProductIds = products
                .Where(p => 
                    p.RemovedOn.HasValue 
                    || !p.Available 
                    || !p.VisibleToConsumers 
                    || p.Producer.RemovedOn.HasValue 
                    || !p.Producer.CanDirectSell)
                .Select(p => p.Id.ToString("N"));

            if (invalidProductIds.Any())
                return Failure<Guid>(MessageKind.Order_CannotPay_Some_Products_NotAvailable,
                    string.Join(";", invalidProductIds));

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var referenceResult = await _identifierService.GetNextOrderReferenceAsync(token);
                if (!referenceResult.Succeeded)
                    return Failure<Guid>(referenceResult.Exception);

                order.SetReference(referenceResult.Data);
                order.SetStatus(OrderStatus.Waiting);
                await _context.SaveChangesAsync(token);

                var result = await _mediatr.Process(new CreateWebPayinCommand(request.RequestUser) {OrderId = order.Id},
                    token);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync(token);
                    return Failure<Guid>(result.Exception);
                }

                await transaction.CommitAsync(token);
                return Success(result.Data);
            }
        }
    }
}