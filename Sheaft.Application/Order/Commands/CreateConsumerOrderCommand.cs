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
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Order.Commands
{
    public class CreateConsumerOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateConsumerOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public DonationKind Donation { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }

    public class CreateConsumerOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreateConsumerOrderCommand, Result<Guid>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly PspOptions _pspOptions;

        public CreateConsumerOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreateConsumerOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateConsumerOrderCommand request, CancellationToken token)
        {
            var productIds = request.Products.Select(p => p.Id);
            var products = await _context.FindByIdsAsync<Domain.Product>(productIds, token);

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
                return Failure<Guid>(MessageKind.Order_CannotCreate_Some_Products_Invalid,
                    string.Join(";", invalidProductIds));

            var cartProducts = new Dictionary<Domain.Product, int>();
            foreach (var product in products)
            {
                cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
            }

            var user = await _context.GetByIdAsync<Domain.User>(request.UserId, token);
            if(user.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            var order = new Domain.Order(Guid.NewGuid(), request.Donation, user, cartProducts, _pspOptions.FixedAmount,
                _pspOptions.Percent, _pspOptions.VatPercent);

            var deliveryIds = request.ProducersExpectedDeliveries?.Select(p => p.DeliveryModeId) ?? new List<Guid>();
            var deliveries = deliveryIds.Any()
                ? await _context.GetByIdsAsync<Domain.DeliveryMode>(deliveryIds, token)
                : new List<Domain.DeliveryMode>();
            var cartDeliveries = new List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>();
            foreach (var delivery in deliveries)
            {
                if(delivery.Closings.Any(c => DateTimeOffset.Now >= c.ClosedFrom && DateTimeOffset.UtcNow <= c.ClosedTo))
                    return Failure<Guid>(MessageKind.Order_CannotCreate_Delivery_Closed, delivery.Name);
                
                var cartDelivery =
                    request.ProducersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);
                cartDeliveries.Add(new Tuple<Domain.DeliveryMode, DateTimeOffset, string>(delivery,
                    cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
            }

            order.SetDeliveries(cartDeliveries);

            await _context.AddAsync(order, token);
            await _context.SaveChangesAsync(token);

            return Success(order.Id);
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