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
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Domains.Exceptions;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CreateConsumerOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateConsumerOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public DonationKind Donation { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }
    public class CreateConsumerOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreateConsumerOrderCommand, Result<Guid>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;

        public CreateConsumerOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CreateConsumerOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateConsumerOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var productIds = request.Products.Select(p => p.Id);
                var products = await _context.FindByIdsAsync<Product>(productIds, token);

                var invalidProductIds = products.Where(p => p.RemovedOn.HasValue || !p.Available || !p.VisibleToConsumers || p.Producer.RemovedOn.HasValue || !p.Producer.CanDirectSell).Select(p => p.Id.ToString("N"));
                if (invalidProductIds.Any())
                    return BadRequest<Guid>(MessageKind.Order_CannotCreate_Some_Products_Invalid, string.Join(";", invalidProductIds));

                var cartProducts = new Dictionary<Product, int>();
                foreach (var product in products)
                {
                    cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
                }

                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var order = new Order(Guid.NewGuid(), request.Donation, user, cartProducts, _pspOptions.FixedAmount, _pspOptions.Percent, _pspOptions.VatPercent);

                var deliveryIds = request.ProducersExpectedDeliveries?.Select(p => p.DeliveryModeId) ?? new List<Guid>();
                var deliveries = deliveryIds.Any() ? await _context.GetByIdsAsync<DeliveryMode>(deliveryIds, token) : new List<DeliveryMode>();
                var cartDeliveries = new List<Tuple<DeliveryMode, DateTimeOffset, string>>();
                foreach (var delivery in deliveries)
                {
                    var cartDelivery = request.ProducersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);
                    cartDeliveries.Add(new Tuple<DeliveryMode, DateTimeOffset, string>(delivery, cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
                }

                order.SetDeliveries(cartDeliveries);

                await _context.AddAsync(order, token);
                await _context.SaveChangesAsync(token);

                return Ok(order.Id);
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
