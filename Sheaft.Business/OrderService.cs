using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Models;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Business
{
    public class OrderService : SheaftService, IOrderService
    {
        private readonly IAppDbContext _context;
        private readonly IDeliveryService _deliveryService;

        public OrderService(
            IAppDbContext context,
            IDeliveryService deliveryService,
            ILogger<OrderService> logger) : base(logger)
        {
            _context = context;
            _deliveryService = deliveryService;
        }

        public async Task<Result> ValidateConsumerOrderAsync(Guid orderId, RequestUser requestUser,
            CancellationToken token)
        {
            var order = await _context.Orders.SingleAsync(e => e.Id == orderId, token);
            if (order.Status != OrderStatus.Created)
                return Failure("Ce panier a déjà été validé.");

            if (order.User == null)
                return Failure("Impossible de valider ce panier, il doit être rattaché à un compte.");

            if (order.UserId != requestUser.Id)
                return Failure("Vous n'avez pas l'autorisation d'accéder à ce panier.");

            if (!order.Deliveries.Any())
                return Failure("Le panier requiert la sélection d'un créneau de récupération.");

            Result result = null;
            foreach (var delivery in order.Deliveries)
            {
                if (delivery.DeliveryMode.Closings.Any(c =>
                    DateTimeOffset.Now >= c.ClosedFrom && DateTimeOffset.UtcNow <= c.ClosedTo))
                {
                    result = Failure($"Le créneau {delivery.DeliveryMode.Name} du producteur {delivery.DeliveryMode.Producer.Name} est fermé à la date sélectionnée.");
                    break;
                }
            }

            if (result is {Succeeded: false})
                return result;

            var validatedDeliveries = await _deliveryService.ValidateCapedDeliveriesAsync(order.Deliveries, token);
            if (!validatedDeliveries.Succeeded)
                return Failure<Guid>(validatedDeliveries.Exception);

            var invalidProductIds = await GetConsumerInvalidProductIds(order.Products.Select(p => p.ProductId), token);
            if (invalidProductIds.Any())
                return Failure($"Certains produits ne sont pas disponibles : {string.Join(';', invalidProductIds)}");

            return Success();
        }

        public async Task<Result<List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>>> GetCartDeliveriesAsync(
            IEnumerable<ProducerExpectedDeliveryInputDto> producersExpectedDeliveries,
            IEnumerable<Tuple<Domain.Product, Guid, int>> cartProducts, CancellationToken token)
        {
            var deliveryIds = producersExpectedDeliveries?.Select(c => c.DeliveryModeId)?.ToList() ?? new List<Guid>();
            var deliveries = deliveryIds.Any()
                ? await _context.DeliveryModes.Where(d => deliveryIds.Contains(d.Id)).ToListAsync(token)
                : new List<Domain.DeliveryMode>();

            var cartDeliveries = new List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>();
            Result<List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>> result = null;
            foreach (var delivery in deliveries)
            {
                if (delivery.Closings.Any(
                    c => DateTimeOffset.Now >= c.ClosedFrom && DateTimeOffset.UtcNow <= c.ClosedTo))
                {
                    result = Failure<List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>>(
                        $"Le créneau {delivery.Name} du producteur {delivery.Producer.Name} est fermé à la date sélectionnée.");

                    break;
                }

                var cartDelivery = producersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);
                if (delivery.Closings.Any(c =>
                    cartDelivery.ExpectedDeliveryDate >= c.ClosedFrom &&
                    cartDelivery.ExpectedDeliveryDate <= c.ClosedTo))
                {
                    result = Failure<List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>>(
                        $"Le créneau {delivery.Name} du producteur {delivery.Producer.Name} est fermé à la date sélectionnée.");

                    break;
                }

                if (cartProducts.Any(p => p.Item1.ProducerId == delivery.ProducerId && p.Item1.Producer.Closings.Any(
                    c =>
                        cartDelivery.ExpectedDeliveryDate >= c.ClosedFrom &&
                        cartDelivery.ExpectedDeliveryDate <= c.ClosedTo)))
                {
                    result = Failure<List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>>(
                        $"Le producteur {delivery.Producer.Name} est fermé à la date selectionnée.");

                    break;
                }

                cartDeliveries.Add(new Tuple<Domain.DeliveryMode, DateTimeOffset, string>(delivery,
                    cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
            }

            if (result is {Succeeded: false})
                return result;

            return Success(cartDeliveries);
        }

        public async Task<Result<List<Tuple<Domain.Product, Guid, int>>>> GetCartProductsAsync(
            IEnumerable<ResourceIdQuantityInputDto> productsQuantities,
            CancellationToken token)
        {
            var productIds = productsQuantities?.Select(c => c.Id)?.ToList() ?? new List<Guid>();
            var invalidProductIds = await GetConsumerInvalidProductIds(productIds, token);
            if (invalidProductIds.Any())
                return Failure<List<Tuple<Domain.Product, Guid, int>>>(
                    $"Certains produits ne sont pas disponibles : {string.Join(';', invalidProductIds)}");

            var cartProducts = new List<Tuple<Domain.Product, Guid, int>>();
            var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync(token);
            Result<List<Tuple<Domain.Product, Guid, int>>> result = null;
            foreach (var product in products)
            {
                var catalog = product.CatalogsPrices.Select(p => p.Catalog).SingleOrDefault(p =>
                    !p.RemovedOn.HasValue && p.Kind == CatalogKind.Consumers && p.IsDefault && p.Available);
                if (catalog == null)
                {
                    result = Failure<List<Tuple<Domain.Product, Guid, int>>>($"Le produit {product.Name} n'est plus disponible à la vente.");
                    break;
                }

                cartProducts.Add(new Tuple<Domain.Product, Guid, int>(product, catalog.Id,
                    productsQuantities.Where(p => p.Id == product.Id).Sum(c => c.Quantity)));
            }

            if (result is {Succeeded: false})
                return result;

            return Success(cartProducts);
        }

        private async Task<IEnumerable<string>> GetConsumerInvalidProductIds(IEnumerable<Guid> productIds,
            CancellationToken token)
        {
            var products = await _context.Products.Where(p => productIds.Contains(p.Id))
                .ToListAsync(token);

            var invalidProductIds = products
                .Where(p => !p.Available
                            || !p.CatalogsPrices.Any(cp => cp.Catalog.Kind == CatalogKind.Consumers)
                            || p.Producer.RemovedOn.HasValue
                            || !p.Producer.CanDirectSell)
                .Select(p => p.Id.ToString("N"))
                .ToList();

            return invalidProductIds;
        }
    }
}