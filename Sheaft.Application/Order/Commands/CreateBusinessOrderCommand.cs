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
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Options;
using Sheaft.Application.PurchaseOrder.Commands;
using Sheaft.Application.User.Commands;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Order.Commands
{
    public class CreateBusinessOrderCommand : Command<IEnumerable<Guid>>
    {
        [JsonConstructor]
        public CreateBusinessOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }

    public class CreateBusinessOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreateBusinessOrderCommand, Result<IEnumerable<Guid>>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly IIdentifierService _identifierService;
        private readonly PspOptions _pspOptions;

        public CreateBusinessOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            IIdentifierService identifierService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreateBusinessOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
            _identifierService = identifierService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(CreateBusinessOrderCommand request, CancellationToken token)
        {
            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var productIds = request.Products.Select(p => p.Id);
                var products = await _context.FindByIdsAsync<Domain.Product>(productIds, token);

                var invalidProductIds = products
                    .Where(p => p.RemovedOn.HasValue 
                                || !p.Available 
                                || !p.VisibleToStores 
                                || p.Producer.RemovedOn.HasValue 
                                || p.Closings.Any(c => DateTimeOffset.Now >= c.ClosedFrom && DateTimeOffset.UtcNow <= c.ClosedTo)
                                || p.Producer.Closings.Any(c => DateTimeOffset.Now >= c.ClosedFrom && DateTimeOffset.UtcNow <= c.ClosedTo))
                    .Select(p => p.Id.ToString("N"));
                if (invalidProductIds.Any())
                    return Failure<IEnumerable<Guid>>(MessageKind.Order_CannotCreate_Some_Products_Invalid,
                        string.Join(";", invalidProductIds));

                var cartProducts = new Dictionary<Domain.Product, int>();
                foreach (var product in products)
                {
                    cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
                }

                var user = await _context.GetByIdAsync<Domain.User>(request.UserId, token);
                if (user.Id != request.RequestUser.Id)
                    throw SheaftException.Forbidden();

                var order = new Domain.Order(Guid.NewGuid(), DonationKind.None, cartProducts,
                    _pspOptions.FixedAmount, _pspOptions.Percent, _pspOptions.VatPercent, user);

                var deliveryIds = request.ProducersExpectedDeliveries.Select(p => p.DeliveryModeId);
                var deliveries = deliveryIds.Any()
                    ? await _context.GetByIdsAsync<Domain.DeliveryMode>(deliveryIds, token)
                    : new List<Domain.DeliveryMode>();
                var cartDeliveries = new List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>();
                foreach (var delivery in deliveries)
                {
                    if(delivery.Closings.Any(c => DateTimeOffset.Now >= c.ClosedFrom && DateTimeOffset.UtcNow <= c.ClosedTo))
                        return Failure<IEnumerable<Guid>>(MessageKind.Order_CannotCreate_Delivery_Closed, delivery.Name);
                        
                    var cartDelivery =
                        request.ProducersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);
                    cartDeliveries.Add(new Tuple<Domain.DeliveryMode, DateTimeOffset, string>(delivery,
                        cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
                }

                if (!cartDeliveries.Any())
                    return Failure<IEnumerable<Guid>>(MessageKind.Order_CannotCreate_Deliveries_Required);

                order.SetDeliveries(cartDeliveries);
                order.SetStatus(OrderStatus.Validated);

                var validatedDeliveries = await ValidateCapedDeliveriesAsync(order.Deliveries, token);
                if (!validatedDeliveries.Succeeded)
                    return Failure<IEnumerable<Guid>>(validatedDeliveries.Exception);

                var referenceResult = await _identifierService.GetNextOrderReferenceAsync(token);
                if (!referenceResult.Succeeded)
                    return Failure<IEnumerable<Guid>>(referenceResult.Exception);

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

                    if (!result.Succeeded)
                        return Failure<IEnumerable<Guid>>(result.Exception);

                    purchaseOrderIds.Add(result.Data);
                }

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                _mediatr.Post(new CreateUserPointsCommand(request.RequestUser)
                {
                    CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = order.User.Id
                });

                return Success(purchaseOrderIds.AsEnumerable());
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