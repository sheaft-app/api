using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Options;

namespace Sheaft.Mediatr.Order.Commands
{
    public class CreateConsumerOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateConsumerOrderCommand(RequestUser requestUser) : base(requestUser)
        {
            UserId = requestUser.IsAuthenticated ? requestUser.Id : (Guid?) null;
        }

        public Guid? UserId { get; set; }
        public DonationKind Donation { get; set; }
        public IEnumerable<ResourceIdQuantityDto> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryDto> ProducersExpectedDeliveries { get; set; }
    }

    public class CreateConsumerOrderCommandHandler : CommandsHandler,
        IRequestHandler<CreateConsumerOrderCommand, Result<Guid>>
    {
        private readonly PspOptions _pspOptions;

        public CreateConsumerOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<CreateConsumerOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateConsumerOrderCommand request, CancellationToken token)
        {
            var productIds = request.Products.Select(p => p.Id);
            var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync(token);
            var invalidProductIds = products
                .Where(p => 
                    p.RemovedOn.HasValue 
                    || !p.Available 
                    || p.CatalogsPrices.Any(cp => cp.Catalog.Kind == CatalogKind.Consumers && cp.Catalog.IsDefault && !cp.Catalog.Available)
                    || p.Producer.RemovedOn.HasValue 
                    || !p.Producer.CanDirectSell)
                .Select(p => p.Id.ToString("N"));
            
            if (invalidProductIds.Any())
                return Failure<Guid>(MessageKind.Order_CannotCreate_Some_Products_NotAvailable,
                    string.Join(";", invalidProductIds));

            var cartProducts = new List<Tuple<Domain.Product, Guid, int>>();
            foreach (var product in products)
            {
                var catalog = product.CatalogsPrices.Select(p => p.Catalog).SingleOrDefault(p => !p.RemovedOn.HasValue && p.Kind == CatalogKind.Consumers && p.IsDefault && p.Available);
                if(catalog == null)
                    throw SheaftException.Validation();
                
                cartProducts.Add(new Tuple<Domain.Product, Guid, int>(product, catalog.Id, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity)));
            }

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
                
                if(delivery.Closings.Any(c => cartDelivery.ExpectedDeliveryDate >= c.ClosedFrom && cartDelivery.ExpectedDeliveryDate <= c.ClosedTo))
                    return Failure<Guid>(MessageKind.Order_CannotCreate_Delivery_Closed, delivery.Name, delivery.Producer.Name);
                    
                if(cartProducts.Any(p => p.Item1.Producer.Id == delivery.Producer.Id && p.Item1.Producer.Closings.Any(c => cartDelivery.ExpectedDeliveryDate >= c.ClosedFrom && cartDelivery.ExpectedDeliveryDate <= c.ClosedTo)))
                    return Failure<Guid>(MessageKind.Order_CannotCreate_Producer_Closed, delivery.Producer.Name);
                
                cartDeliveries.Add(new Tuple<Domain.DeliveryMode, DateTimeOffset, string>(delivery,
                    cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
            }

            Domain.User user = request.UserId.HasValue && request.UserId != Guid.Empty ? await _context.GetByIdAsync<Domain.User>(request.UserId.Value, token) : null;
            if(user != null && user.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            var order = new Domain.Order(Guid.NewGuid(), request.Donation, cartProducts, _pspOptions.FixedAmount,
                _pspOptions.Percent, _pspOptions.VatPercent, user);
            
            order.SetDeliveries(cartDeliveries);

            await _context.AddAsync(order, token);
            await _context.SaveChangesAsync(token);

            return Success(order.Id);
        }
    }
}
