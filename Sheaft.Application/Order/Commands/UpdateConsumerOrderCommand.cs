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
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class UpdateConsumerOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateConsumerOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid Id { get; set; }
        public DonationKind Donation { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }
    public class UpdateConsumerOrderCommandHandler : CommandsHandler,
        IRequestHandler<UpdateConsumerOrderCommand, Result<bool>>
    {
        private readonly ICapingDeliveriesService _capingDeliveriesService;
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;

        public UpdateConsumerOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ICapingDeliveriesService capingDeliveriesService,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<UpdateConsumerOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _capingDeliveriesService = capingDeliveriesService;
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<bool>> Handle(UpdateConsumerOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var entity = await _context.GetByIdAsync<Order>(request.Id, token);

                var productIds = request.Products.Select(p => p.Id);
                var products = await _context.FindByIdsAsync<Product>(productIds, token);

                var invalidProductIds = products.Where(p => p.RemovedOn.HasValue || !p.Available || !p.VisibleToConsumers || p.Producer.RemovedOn.HasValue || !p.Producer.CanDirectSell).Select(p => p.Id.ToString("N"));
                if (invalidProductIds.Any())
                    return BadRequest<bool>(MessageKind.Order_CannotUpdate_Some_Products_Invalid, string.Join(";", invalidProductIds));

                var cartProducts = new Dictionary<Product, int>();
                foreach (var product in products)
                {
                    cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
                }
                entity.SetProducts(cartProducts);

                var deliveryIds = request.ProducersExpectedDeliveries?.Select(p => p.DeliveryModeId) ?? new List<Guid>();
                var deliveries = deliveryIds.Any() ? await _context.GetByIdsAsync<DeliveryMode>(deliveryIds, token) : new List<DeliveryMode>();
                var cartDeliveries = new List<Tuple<DeliveryMode, DateTimeOffset, string>>();
                foreach (var delivery in deliveries)
                {
                    var cartDelivery = request.ProducersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);
                    cartDeliveries.Add(new Tuple<DeliveryMode, DateTimeOffset, string>(delivery, cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
                }

                entity.SetDeliveries(cartDeliveries);
                entity.SetDonation(request.Donation);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }
    }
}
