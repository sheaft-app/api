using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using System.Linq;
using System.Collections.Generic;
using Sheaft.Domain.Enums;
using Sheaft.Application.Events;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Sheaft.Exceptions;

namespace Sheaft.Application.Handlers
{
    public class OrderCommandsHandler : ResultsHandler,
        IRequestHandler<CreateConsumerOrderCommand, Result<Guid>>,
        IRequestHandler<CreateBusinessOrderCommand, Result<IEnumerable<Guid>>>,
        IRequestHandler<UpdateConsumerOrderCommand, Result<bool>>,
        IRequestHandler<PayOrderCommand, Result<Guid>>,
        IRequestHandler<ConfirmOrderCommand, Result<IEnumerable<Guid>>>
    {
        private readonly PspOptions _pspOptions;

        public OrderCommandsHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<OrderCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateConsumerOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var productIds = request.Products.Select(p => p.Id);
                var products = await _context.GetByIdsAsync<Product>(productIds, token);
                var cartProducts = new Dictionary<Product, int>();
                foreach (var product in products)
                {
                    cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
                }

                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var order = new Order(Guid.NewGuid(), request.Donation, user, cartProducts, _pspOptions.FixedAmount, _pspOptions.Percent);

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

        public async Task<Result<IEnumerable<Guid>>> Handle(CreateBusinessOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var productIds = request.Products.Select(p => p.Id);
                    var products = await _context.GetByIdsAsync<Product>(productIds, token);
                    var cartProducts = new Dictionary<Product, int>();
                    foreach (var product in products)
                    {
                        cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
                    }

                    var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                    var order = new Order(Guid.NewGuid(), DonationKind.None, user, cartProducts, _pspOptions.FixedAmount, _pspOptions.Percent);

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

                    var orderIds = new List<Guid>();
                    var producerIds = order.Products.Select(p => p.Producer.Id).Distinct();
                    foreach (var producerId in producerIds)
                    {
                        var result = await _mediatr.Process(new CreatePurchaseOrderCommand(request.RequestUser)
                        {
                            OrderId = order.Id,
                            ProducerId = producerId,
                            SkipSendEmail = true
                        }, token);

                        if (!result.Success)
                            return Failed<IEnumerable<Guid>>(result.Exception);

                        orderIds.Add(result.Data);
                    }

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    foreach (var orderId in orderIds)
                        await _mediatr.Post(new PurchaseOrderCreatedEvent(request.RequestUser) { Id = orderId }, token);

                    await _mediatr.Post(new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = request.RequestUser.Id }, token);

                    await transaction.CommitAsync(token);
                    return Ok(orderIds.AsEnumerable());
                }
            });
        }

        public async Task<Result<bool>> Handle(UpdateConsumerOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var entity = await _context.GetByIdAsync<Order>(request.Id, token);

                var productIds = request.Products.Select(p => p.Id);
                var products = await _context.GetByIdsAsync<Product>(productIds, token);
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
                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<Guid>> Handle(PayOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var checkResult = await _mediatr.Process(new EnsureConsumerConfiguredCommand(request.RequestUser) { Id = request.RequestUser.Id }, token);
                if (!checkResult.Success)
                    return Failed<Guid>(checkResult.Exception);

                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var order = await _context.GetByIdAsync<Order>(request.Id, token);
                    if (!order.Deliveries.Any())
                        return Failed<Guid>(new ValidationException());

                    order.SetStatus(OrderStatus.Waiting);

                    _context.Update(order);
                    await _context.SaveChangesAsync(token);

                    var result = await _mediatr.Process(new CreateWebPayInTransactionCommand(request.RequestUser) { OrderId = order.Id }, token);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(token);
                        return Failed<Guid>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(result.Data);
                }
            });
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(ConfirmOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var order = await _context.GetByIdAsync<Order>(request.Id, token);
                    var orderIds = new List<Guid>();

                    order.SetStatus(OrderStatus.Validated);

                    var producerIds = order.Products.Select(p => p.Producer.Id).Distinct();
                    foreach(var producerId in producerIds)
                    {
                        var result = await _mediatr.Process(new CreatePurchaseOrderCommand(request.RequestUser)
                        {
                            OrderId = order.Id,
                            ProducerId = producerId,
                            SkipSendEmail = true
                        }, token);

                        if (!result.Success)
                            return Failed<IEnumerable<Guid>>(result.Exception);

                        orderIds.Add(result.Data);
                    }

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    foreach (var orderId in orderIds)
                        await _mediatr.Post(new PurchaseOrderCreatedEvent(request.RequestUser) { Id = orderId }, token);

                    await _mediatr.Post(new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = request.RequestUser.Id }, token);
                    return Ok(orderIds.AsEnumerable());
                }
            });
        }
    }
}
