using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Common;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Options;
using Sheaft.Application.Consumer.Commands;
using Sheaft.Application.Payin.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Order.Commands
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
        private readonly ICapingDeliveriesService _capingDeliveriesService;

        public PayOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IIdentifierService identifierService,
            ICapingDeliveriesService capingDeliveriesService,
            ILogger<PayOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
            _capingDeliveriesService = capingDeliveriesService;
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

            var validatedDeliveries = await ValidateCapedDeliveriesAsync(order.Deliveries, token);
            if (!validatedDeliveries.Succeeded)
                return Failure<Guid>(validatedDeliveries.Exception);

            var products = await _context.GetByIdsAsync<Domain.Product>(order.Products.Select(p => p.Id), token);
            var invalidProductIds = products
                .Where(p => p.RemovedOn.HasValue 
                            || !p.Available 
                            || !p.VisibleToConsumers 
                            || p.Producer.RemovedOn.HasValue 
                            || !p.Producer.CanDirectSell
                            || p.Closings.Any(c => DateTimeOffset.Now >= c.ClosedFrom && DateTimeOffset.UtcNow <= c.ClosedTo)
                            || p.Producer.Closings.Any(c => DateTimeOffset.Now >= c.ClosedFrom && DateTimeOffset.UtcNow <= c.ClosedTo))
                .Select(p => p.Id.ToString("N"));

            if (invalidProductIds.Any())
                return Failure<Guid>(MessageKind.Order_CannotPay_Some_Products_Invalid,
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

        private async Task<Result<bool>> ValidateCapedDeliveriesAsync(
            IReadOnlyCollection<OrderDelivery> orderDeliveries, CancellationToken token)
        {
            if (orderDeliveries.All(d => !d.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue))
                return Success(true);

            var results = await _capingDeliveriesService.GetCapingDeliveriesAsync(
                orderDeliveries.Where(d => d.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue).Select(d =>
                    new Tuple<Guid, Guid, DeliveryHourDto>(
                        d.DeliveryMode.Producer.Id,
                        d.DeliveryMode.Id,
                        new DeliveryHourDto
                        {
                            Day = d.ExpectedDelivery.ExpectedDeliveryDate.DayOfWeek,
                            ExpectedDeliveryDate = d.ExpectedDelivery.ExpectedDeliveryDate,
                            From = d.ExpectedDelivery.From,
                            To = d.ExpectedDelivery.To,
                        })
                ), token);

            if (!results.Succeeded)
                return Failure<bool>(results.Exception);

            foreach (var orderDelivery in orderDeliveries)
            {
                var delivery = results.Data.FirstOrDefault(d => d.DeliveryId == orderDelivery.Id
                                                                && d.ExpectedDate.Year == orderDelivery.ExpectedDelivery
                                                                    .ExpectedDeliveryDate.Year
                                                                && d.ExpectedDate.Month ==
                                                                orderDelivery.ExpectedDelivery.ExpectedDeliveryDate
                                                                    .Month
                                                                && d.ExpectedDate.Day == orderDelivery.ExpectedDelivery
                                                                    .ExpectedDeliveryDate.Day
                                                                && d.From == orderDelivery.ExpectedDelivery.From
                                                                && d.To == orderDelivery.ExpectedDelivery.To);

                if (delivery == null)
                    continue;

                if (delivery.Count >= orderDelivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot)
                    return Failure<bool>(new ValidationException(
                        MessageKind.Order_CannotPay_Delivery_Max_PurchaseOrders_Reached,
                        orderDelivery.DeliveryMode.Producer.Name,
                        $"le {orderDelivery.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy} entre {orderDelivery.ExpectedDelivery.From:hh\\hmm} et {orderDelivery.ExpectedDelivery.To:hh\\hmm}",
                        orderDelivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot));
            }

            return Success(true);
        }
    }
}