using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Domains.Exceptions;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
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
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<PayOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
            _capingDeliveriesService = capingDeliveriesService;
        }

        public async Task<Result<Guid>> Handle(PayOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var checkResult = await _mediatr.Process(new CheckConsumerConfigurationCommand(request.RequestUser) { Id = request.RequestUser.Id }, token);
                if (!checkResult.Success)
                    return Failed<Guid>(checkResult.Exception);

                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                if (!order.Deliveries.Any())
                    return BadRequest<Guid>(MessageKind.Order_CannotPay_Deliveries_Required);

                var validatedDeliveries = await ValidateCapedDeliveriesAsync(order.Deliveries, token);
                if (!validatedDeliveries.Success)
                    return Failed<Guid>(validatedDeliveries.Exception);

                var products = await _context.GetByIdsAsync<Product>(order.Products.Select(p => p.Id), token);
                var invalidProductIds = products.Where(p => p.RemovedOn.HasValue || !p.Available || !p.VisibleToConsumers || p.Producer.RemovedOn.HasValue || !p.Producer.CanDirectSell).Select(p => p.Id.ToString("N"));

                if (invalidProductIds.Any())
                    return BadRequest<Guid>(MessageKind.Order_CannotPay_Some_Products_Invalid, string.Join(";", invalidProductIds));

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var referenceResult = await _identifierService.GetNextOrderReferenceAsync(token);
                    if (!referenceResult.Success)
                        return Failed<Guid>(referenceResult.Exception);

                    order.SetReference(referenceResult.Data);
                    order.SetStatus(OrderStatus.Waiting);
                    await _context.SaveChangesAsync(token);

                    var result = await _mediatr.Process(new CreateWebPayinCommand(request.RequestUser) { OrderId = order.Id }, token);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(token);
                        return Failed<Guid>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(result.Data);
                }
            });
        }

        private async Task<Result<bool>> ValidateCapedDeliveriesAsync(IReadOnlyCollection<OrderDelivery> orderDeliveries, CancellationToken token)
        {
            if (orderDeliveries.All(d => !d.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue))
                return Ok(true);

            var results = await _capingDeliveriesService.GetCapingDeliveriesAsync(
                orderDeliveries.Where(d => d.DeliveryMode.MaxPurchaseOrdersPerTimeSlot.HasValue).Select(d =>
                    new Tuple<Guid, Guid, Models.DeliveryHourDto>(
                        d.DeliveryMode.Producer.Id,
                        d.DeliveryMode.Id,
                        new Models.DeliveryHourDto
                        {
                            Day = d.ExpectedDelivery.ExpectedDeliveryDate.DayOfWeek,
                            ExpectedDeliveryDate = d.ExpectedDelivery.ExpectedDeliveryDate,
                            From = d.ExpectedDelivery.From,
                            To = d.ExpectedDelivery.To,
                        })
                    ), token);

            if (!results.Success)
                return Failed<bool>(results.Exception);

            foreach (var orderDelivery in orderDeliveries)
            {
                var delivery = results.Data.FirstOrDefault(d => d.DeliveryId == orderDelivery.Id 
                    && d.ExpectedDate.Year == orderDelivery.ExpectedDelivery.ExpectedDeliveryDate.Year 
                    && d.ExpectedDate.Month == orderDelivery.ExpectedDelivery.ExpectedDeliveryDate.Month 
                    && d.ExpectedDate.Day == orderDelivery.ExpectedDelivery.ExpectedDeliveryDate.Day
                    && d.From == orderDelivery.ExpectedDelivery.From
                    && d.To == orderDelivery.ExpectedDelivery.To);

                if (delivery == null)
                    continue;

                if (delivery.Count >= orderDelivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot)
                    return Failed<bool>(new ValidationException(MessageKind.Order_CannotPay_Delivery_Max_PurchaseOrders_Reached, orderDelivery.DeliveryMode.Producer.Name, $"le {orderDelivery.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy} entre {orderDelivery.ExpectedDelivery.From:hh\\hmm} et {orderDelivery.ExpectedDelivery.To:hh\\hmm}", orderDelivery.DeliveryMode.MaxPurchaseOrdersPerTimeSlot));
            }

            return Ok(true);
        }
    }
}
