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
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Order.Commands
{
    public class UpdateConsumerOrderCommand : Command
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
        IRequestHandler<UpdateConsumerOrderCommand, Result>
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

        public async Task<Result> Handle(UpdateConsumerOrderCommand request, CancellationToken token)
        {
            var user = await _context.GetByIdAsync<Domain.User>(request.RequestUser.Id, token);
            var entity = await _context.GetByIdAsync<Domain.Order>(request.Id, token);

            var productIds = request.Products.Select(p => p.Id);
            var products = await _context.FindByIdsAsync<Domain.Product>(productIds, token);

            var invalidProductIds = products
                .Where(p => p.RemovedOn.HasValue || !p.Available || !p.VisibleToConsumers ||
                            p.Producer.RemovedOn.HasValue || !p.Producer.CanDirectSell).Select(p => p.Id.ToString("N"));
            if (invalidProductIds.Any())
                return Failure(MessageKind.Order_CannotUpdate_Some_Products_Invalid,
                    string.Join(";", invalidProductIds));

            var cartProducts = new Dictionary<Domain.Product, int>();
            foreach (var product in products)
            {
                cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
            }

            entity.SetProducts(cartProducts);

            var deliveryIds = request.ProducersExpectedDeliveries?.Select(p => p.DeliveryModeId) ?? new List<Guid>();
            var deliveries = deliveryIds.Any()
                ? await _context.GetByIdsAsync<Domain.DeliveryMode>(deliveryIds, token)
                : new List<Domain.DeliveryMode>();
            var cartDeliveries = new List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>();
            foreach (var delivery in deliveries)
            {
                var cartDelivery =
                    request.ProducersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);
                cartDeliveries.Add(new Tuple<Domain.DeliveryMode, DateTimeOffset, string>(delivery,
                    cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
            }

            entity.SetDeliveries(cartDeliveries);
            entity.SetDonation(request.Donation);

            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}