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
using Sheaft.Application.Common;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
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
                    || !p.VisibleToConsumers 
                    || p.Producer.RemovedOn.HasValue 
                    || !p.Producer.CanDirectSell)
                .Select(p => p.Id.ToString("N"));
            
            if (invalidProductIds.Any())
                return Failure<Guid>(MessageKind.Order_CannotCreate_Some_Products_NotAvailable,
                    string.Join(";", invalidProductIds));

            var cartProducts = new Dictionary<Domain.Product, int>();
            foreach (var product in products)
            {
                cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
            }

            Domain.User user = request.UserId != Guid.Empty ? await _context.GetByIdAsync<Domain.User>(request.UserId, token) : null;
            if(user != null && user.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            var order = new Domain.Order(Guid.NewGuid(), request.Donation, cartProducts, _pspOptions.FixedAmount,
                _pspOptions.Percent, _pspOptions.VatPercent, user);

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
                    
                if(cartProducts.Any(p => p.Key.Producer.Id == delivery.Producer.Id && p.Key.Producer.Closings.Any(c => cartDelivery.ExpectedDeliveryDate >= c.ClosedFrom && cartDelivery.ExpectedDeliveryDate <= c.ClosedTo)))
                    return Failure<Guid>(MessageKind.Order_CannotCreate_Producer_Closed, delivery.Producer.Name);

                invalidProductIds = cartProducts.Where(p =>
                        p.Key.Producer.Id == delivery.Producer.Id && p.Key.Closings.Any(c =>
                            cartDelivery.ExpectedDeliveryDate >= c.ClosedFrom &&
                            cartDelivery.ExpectedDeliveryDate <= c.ClosedTo))
                    .Select(p => p.Key.Id.ToString("N"));
                    
                if(invalidProductIds.Any())
                    return Failure<Guid>(MessageKind.Order_CannotCreate_Some_Products_Closed,
                        string.Join(";", invalidProductIds));
                
                cartDeliveries.Add(new Tuple<Domain.DeliveryMode, DateTimeOffset, string>(delivery,
                    cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
            }

            order.SetDeliveries(cartDeliveries);

            await _context.AddAsync(order, token);
            await _context.SaveChangesAsync(token);

            return Success(order.Id);
        }
    }
}