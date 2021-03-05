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
using Sheaft.Domain.Exceptions;

namespace Sheaft.Application.Order.Commands
{
    public class UpdateConsumerOrderCommand : Command
    {
        [JsonConstructor]
        public UpdateConsumerOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid? UserId { get; set; }
        public Guid OrderId { get; set; }
        public DonationKind Donation { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }

    public class UpdateConsumerOrderCommandHandler : CommandsHandler,
        IRequestHandler<UpdateConsumerOrderCommand, Result>
    {
        public UpdateConsumerOrderCommandHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<UpdateConsumerOrderCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateConsumerOrderCommand request, CancellationToken token)
        {
            var entity = await _context.GetByIdAsync<Domain.Order>(request.OrderId, token);
            if(entity.User != null && entity.User.Id != request.RequestUser.Id)
                throw SheaftException.Forbidden();
            
            if(entity.User != null && request.UserId.HasValue && request.UserId.Value != entity.User.Id)
                throw SheaftException.Conflict();

            if (entity.User == null && request.UserId.HasValue && request.UserId != Guid.Empty)
            {
                var user = await _context.GetByIdAsync<Domain.User>(request.UserId.Value, token);
                entity.AssignToUser(user);
            }

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
                if(delivery.Closings.Any(c => DateTimeOffset.Now >= c.ClosedFrom && DateTimeOffset.UtcNow <= c.ClosedTo))
                    return Failure<IEnumerable<Guid>>(MessageKind.Order_CannotCreate_Delivery_Closed, delivery.Name);

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