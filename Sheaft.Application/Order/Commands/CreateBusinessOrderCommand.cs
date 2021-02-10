using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Domains.Exceptions;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CreateBusinessOrderCommand : Command<IEnumerable<Guid>>
    {
        [JsonConstructor]
        public CreateBusinessOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }
    public class CreateBusinessOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreateBusinessOrderCommand, Result<IEnumerable<Guid>>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly IIdentifierService _identifierService;
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;

        public CreateBusinessOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            IIdentifierService identifierService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CreateBusinessOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
            _identifierService = identifierService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(CreateBusinessOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var productIds = request.Products.Select(p => p.Id);
                    var products = await _context.FindByIdsAsync<Product>(productIds, token);

                    var invalidProductIds = products.Where(p => p.RemovedOn.HasValue || !p.Available || !p.VisibleToStores || p.Producer.RemovedOn.HasValue || !p.Producer.CanDirectSell).Select(p => p.Id.ToString("N"));
                    if (invalidProductIds.Any())
                        return BadRequest<IEnumerable<Guid>>(MessageKind.Order_CannotCreate_Some_Products_Invalid, string.Join(";", invalidProductIds));

                    var cartProducts = new Dictionary<Product, int>();
                    foreach (var product in products)
                    {
                        cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
                    }

                    var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                    var order = new Order(Guid.NewGuid(), DonationKind.None, user, cartProducts, _pspOptions.FixedAmount, _pspOptions.Percent, _pspOptions.VatPercent);

                    var deliveryIds = request.ProducersExpectedDeliveries?.Select(p => p.DeliveryModeId) ?? new List<Guid>();
                    var deliveries = deliveryIds.Any() ? await _context.GetByIdsAsync<DeliveryMode>(deliveryIds, token) : new List<DeliveryMode>();
                    var cartDeliveries = new List<Tuple<DeliveryMode, DateTimeOffset, string>>();
                    foreach (var delivery in deliveries)
                    {
                        var cartDelivery = request.ProducersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);
                        cartDeliveries.Add(new Tuple<DeliveryMode, DateTimeOffset, string>(delivery, cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
                    }

                    if (!cartDeliveries.Any())
                        return BadRequest<IEnumerable<Guid>>(MessageKind.Order_CannotCreate_Deliveries_Required);

                    order.SetDeliveries(cartDeliveries);
                    order.SetStatus(OrderStatus.Validated);

                    var validatedDeliveries = await ValidateCapedDeliveriesAsync(order.Deliveries, token);
                    if (!validatedDeliveries.Success)
                        return Failed<IEnumerable<Guid>>(validatedDeliveries.Exception);

                    var referenceResult = await _identifierService.GetNextOrderReferenceAsync(token);
                    if (!referenceResult.Success)
                        return Failed<IEnumerable<Guid>>(referenceResult.Exception);

                    order.SetReference(referenceResult.Data);

                    await _context.AddAsync(order, token);
                    await _context.SaveChangesAsync(token);

                    var purchaseOrderIds = new List<Guid>();
                    var producerIds = order.Products.Select(p => p.Producer.Id).Distinct();
                    foreach (var producerId in producerIds)
                    {
                        var result = await _mediatr.Process(new CreatePurchaseOrderCommand(request.RequestUser)
                        {
                            OrderId = order.Id,
                            ProducerId = producerId,
                            SkipNotification = true
                        }, token);

                        if (!result.Success)
                            return Failed<IEnumerable<Guid>>(result.Exception);

                        purchaseOrderIds.Add(result.Data);
                    }

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    foreach (var purchaseOrderId in purchaseOrderIds)
                    {
                        _mediatr.Post(new PurchaseOrderCreatedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrderId });
                        _mediatr.Post(new PurchaseOrderReceivedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrderId });
                    }

                    _mediatr.Post(new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = order.User.Id });
                    return Ok(purchaseOrderIds.AsEnumerable());
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
