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
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class OrderCommandsHandler : ResultsHandler,
        IRequestHandler<CreateConsumerOrderCommand, Result<Guid>>,
        IRequestHandler<CreateBusinessOrderCommand, Result<IEnumerable<Guid>>>,
        IRequestHandler<UpdateConsumerOrderCommand, Result<bool>>,
        IRequestHandler<PayOrderCommand, Result<Guid>>,
        IRequestHandler<RetryOrderCommand, Result<Guid>>,
        IRequestHandler<ConfirmOrderCommand, Result<IEnumerable<Guid>>>,
        IRequestHandler<FailOrderCommand, Result<bool>>,
        IRequestHandler<ExpireOrderCommand, Result<bool>>,
        IRequestHandler<UnblockOrderCommand, Result<bool>>,
        IRequestHandler<CheckOrdersCommand, Result<bool>>,
        IRequestHandler<CheckOrderCommand, Result<bool>>
    {
        private readonly PspOptions _pspOptions;
        private readonly RoutineOptions _routineOptions;

        public OrderCommandsHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<PspOptions> pspOptions,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<OrderCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspOptions = pspOptions.Value;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateConsumerOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var productIds = request.Products.Select(p => p.Id);
                var products = await _context.GetByIdsAsync<Product>(productIds, token);
                if (products.Any(p => !p.Available))
                    return Failed<Guid>(new ValidationException());

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
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var productIds = request.Products.Select(p => p.Id);
                    var products = await _context.GetByIdsAsync<Product>(productIds, token);
                    if(products.Any(p => !p.Available))
                        return Failed<IEnumerable<Guid>>(new ValidationException());

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

                    if(!cartDeliveries.Any())
                        return Failed<IEnumerable<Guid>>(new ValidationException());

                    order.SetDeliveries(cartDeliveries);
                    order.SetStatus(OrderStatus.Validated);

                    var referenceResult = await _mediatr.Process(new CreateOrderIdentifierCommand(request.RequestUser), token);
                    if (!referenceResult.Success)
                        return Failed<IEnumerable<Guid>>(referenceResult.Exception);

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
                            SkipSendEmail = true
                        }, token);

                        if (!result.Success)
                            return Failed<IEnumerable<Guid>>(result.Exception);

                        purchaseOrderIds.Add(result.Data);
                    }

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    foreach (var purchaseOrderId in purchaseOrderIds)
                    {
                        _mediatr.Post(new PurchaseOrderCreatedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrderId });
                        _mediatr.Post(new PurchaseOrderReceivedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrderId });
                    }

                    _mediatr.Post(new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = order.User.Id });
                    return Ok(purchaseOrderIds.AsEnumerable());
                }
            });
        }

        public async Task<Result<bool>> Handle(UpdateConsumerOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
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

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<Guid>> Handle(PayOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var checkResult = await _mediatr.Process(new CheckConsumerConfigurationCommand(request.RequestUser) { Id = request.RequestUser.Id }, token);
                if (!checkResult.Success)
                    return Failed<Guid>(checkResult.Exception);

                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                if (!order.Deliveries.Any())
                    return Failed<Guid>(new ValidationException());

                var products = await _context.GetByIdsAsync<Product>(order.Products.Select(p => p.Id), token);
                if (products.Any(p => !p.Available))
                    return Failed<Guid>(new ValidationException());

                if (products.Any(p => !p.Searchable))
                    return Failed<Guid>(new ValidationException());

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var referenceResult = await _mediatr.Process(new CreateOrderIdentifierCommand(request.RequestUser), token);
                    if (!referenceResult.Success)
                        return Failed<Guid>(referenceResult.Exception);

                    order.SetReference(referenceResult.Data);
                    order.SetStatus(OrderStatus.Waiting);
                    await _context.SaveChangesAsync(token);

                    var result = await _mediatr.Process(new CreateWebPayinCommand(request.RequestUser) { OrderId = order.Id }, token);
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

        public async Task<Result<Guid>> Handle(RetryOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                if (order.Status != OrderStatus.Refused)
                    return Failed<Guid>(new InvalidOperationException());

                var checkResult = await _mediatr.Process(new CheckConsumerConfigurationCommand(request.RequestUser) { Id = request.RequestUser.Id }, token);
                if (!checkResult.Success)
                    return Failed<Guid>(checkResult.Exception);

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    order.SetStatus(OrderStatus.Waiting);
                    await _context.SaveChangesAsync(token);

                    var result = await _mediatr.Process(new CreateWebPayinCommand(request.RequestUser) { OrderId = order.Id }, token);
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
            return await ExecuteAsync(request, async () =>
            {
                try
                {
                    using (var transaction = await _context.BeginTransactionAsync(token))
                    {
                        var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                        var purchaseOrderIds = new List<Guid>();

                        order.SetStatus(OrderStatus.Validated);

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

                            purchaseOrderIds.Add(result.Data);
                        }

                        await _context.SaveChangesAsync(token);
                        await transaction.CommitAsync(token);

                        foreach (var purchaseOrderId in purchaseOrderIds)
                        {
                            _mediatr.Post(new PurchaseOrderCreatedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrderId });
                            _mediatr.Post(new PurchaseOrderReceivedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrderId });
                        }

                        _mediatr.Post(new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = order.User.Id });
                        _mediatr.Schedule(new CreateDonationCommand(request.RequestUser) { OrderId = order.Id }, TimeSpan.FromMinutes(60));

                        return Ok(purchaseOrderIds.AsEnumerable());
                    }
                }
                catch (Exception e)
                {
                    _mediatr.Post(new ConfirmOrderFailedEvent(request.RequestUser) { OrderId = request.OrderId, Message = e.Message });
                    throw;
                }
            });
        }

        public async Task<Result<bool>> Handle(FailOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                if (order.Payin.Id != request.PayinId)
                    return Ok(true);

                order.SetStatus(OrderStatus.Refused);
                await _context.SaveChangesAsync(token);

                _mediatr.Post(new PayinFailedEvent(request.RequestUser) { PayinId = request.PayinId });
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ExpireOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                order.SetStatus(OrderStatus.Expired);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(UnblockOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                order.SetSkipBackgroundProcessing(false);

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var skip = 0;
                const int take = 100;

                var expiredDate = DateTimeOffset.UtcNow.AddMinutes(-_routineOptions.CheckOrdersFromMinutes);
                var orderIds = await GetNextOrderIdsAsync(expiredDate, skip, take, token);

                while (orderIds.Any())
                {
                    foreach (var orderId in orderIds)
                    {
                        _mediatr.Post(new CheckOrderCommand(request.RequestUser)
                        {
                            OrderId = orderId
                        });
                    }

                    skip += take;
                    orderIds = await GetNextOrderIdsAsync(expiredDate, skip, take, token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CheckOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var payinRefund = await _context.GetByIdAsync<Order>(request.OrderId, token);
                if (payinRefund.Status != OrderStatus.Created && payinRefund.Status != OrderStatus.Waiting)
                    return Ok(false);

                if (payinRefund.CreatedOn.AddMinutes(_routineOptions.CheckOrderExpiredFromMinutes) < DateTimeOffset.UtcNow)
                    return await _mediatr.Process(new ExpireOrderCommand(request.RequestUser) { OrderId = request.OrderId }, token);

                return Ok(true);
            });
        }

        private async Task<IEnumerable<Guid>> GetNextOrderIdsAsync(DateTimeOffset expiredDate, int skip, int take, CancellationToken token)
        {
            return await _context.Orders
                .Get(c => c.CreatedOn < expiredDate
                        && (c.Payin == null || c.Payin.Status == TransactionStatus.Failed || c.Payin.Status == TransactionStatus.Expired)
                        && (c.Status == OrderStatus.Waiting || c.Status == OrderStatus.Created), true)
                .OrderBy(c => c.CreatedOn)
                .Select(c => c.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }
    }
}
