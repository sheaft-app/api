using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Infrastructure.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using System.Linq;
using System.Collections.Generic;
using Sheaft.Services.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Application.Events;
using Sheaft.Options;
using Microsoft.Extensions.Options;

namespace Sheaft.Application.Handlers
{
    public class OrderCommandsHandler : ResultsHandler,
        IRequestHandler<CreateConsumerOrderCommand, Result<Guid>>,
        IRequestHandler<CreateBusinessOrderCommand, Result<IEnumerable<Guid>>>,
        IRequestHandler<UpdateConsumerOrderCommand, Result<bool>>,
        IRequestHandler<PayOrderCommand, Result<Guid>>,
        IRequestHandler<ConfirmOrderCommand, Result<IEnumerable<Guid>>>
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediatr;
        private readonly IQueueService _queuesService;
        private readonly PspOptions _pspOptions;

        public OrderCommandsHandler(
            IAppDbContext context,
            IMediator mediatr,
            IQueueService queuesService,
            IOptionsSnapshot<PspOptions> pspOptions,
            ILogger<OrderCommandsHandler> logger) : base(logger)
        {
            _context = context;
            _mediatr = mediatr;
            _queuesService = queuesService;
            _pspOptions = pspOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateConsumerOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var productIds = request.Products.Select(p => p.Id);
                var deliveryIds = request.ProducersExpectedDeliveries.Select(p => p.DeliveryModeId);
                var products = await _context.GetByIdsAsync<Product>(productIds, token);
                var deliveries = await _context.GetByIdsAsync<DeliveryMode>(deliveryIds, token);

                var cartProducts = new Dictionary<Product, int>();
                foreach (var product in products)
                {
                    cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
                }

                var cartDeliveries = new List<Tuple<DeliveryMode, DateTimeOffset, string>>();
                foreach (var delivery in deliveries)
                {
                    var cartDelivery = request.ProducersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);
                    cartDeliveries.Add(new Tuple<DeliveryMode, DateTimeOffset, string>(delivery, cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
                }

                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var order = new Order(Guid.NewGuid(), user, cartProducts, cartDeliveries);
                order.SetDonation(request.Donation);
                order.RefreshFees(_pspOptions.FixedAmount, _pspOptions.Percent);

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
                    var deliveryIds = request.ProducersExpectedDeliveries.Select(p => p.DeliveryModeId);
                    var products = await _context.GetByIdsAsync<Product>(productIds, token);
                    var deliveries = await _context.GetByIdsAsync<DeliveryMode>(deliveryIds, token);

                    var cartProducts = new Dictionary<Product, int>();
                    foreach (var product in products)
                    {
                        cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
                    }

                    var cartDeliveries = new List<Tuple<DeliveryMode, DateTimeOffset, string>>();
                    foreach (var delivery in deliveries)
                    {
                        var cartDelivery = request.ProducersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);
                        cartDeliveries.Add(new Tuple<DeliveryMode, DateTimeOffset, string>(delivery, cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
                    }

                    var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                    var order = new Order(Guid.NewGuid(), user, cartProducts, cartDeliveries);

                    await _context.AddAsync(order, token);
                    await _context.SaveChangesAsync(token);

                    var orderIds = new List<Guid>();
                    var producerIds = order.Products.Select(p => p.Producer.Id).Distinct();
                    foreach (var producerId in producerIds)
                    {
                        var result = await _mediatr.Send(new CreatePurchaseOrderCommand(request.RequestUser)
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
                        await _queuesService.ProcessEventAsync(PurchaseOrderCreatedEvent.QUEUE_NAME, new PurchaseOrderCreatedEvent(request.RequestUser) { Id = orderId }, token);

                    await _queuesService.ProcessCommandAsync(CreateUserPointsCommand.QUEUE_NAME, new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = request.RequestUser.Id }, token);

                    await transaction.CommitAsync(token);
                    return Ok(orderIds.AsEnumerable());
                }
            });
        }

        public async Task<Result<bool>> Handle(UpdateConsumerOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var productIds = request.Products.Select(p => p.Id);
                var deliveryIds = request.ProducersExpectedDeliveries.Select(p => p.DeliveryModeId);
                var products = await _context.GetByIdsAsync<Product>(productIds, token);
                var deliveries = await _context.GetByIdsAsync<DeliveryMode>(deliveryIds, token);

                var cartProducts = new Dictionary<Product, int>();
                foreach (var product in products)
                {
                    cartProducts.Add(product, request.Products.Where(p => p.Id == product.Id).Sum(c => c.Quantity));
                }

                var cartDeliveries = new List<Tuple<DeliveryMode, DateTimeOffset, string>>();
                foreach (var delivery in deliveries)
                {
                    var cartDelivery = request.ProducersExpectedDeliveries.FirstOrDefault(ped => ped.DeliveryModeId == delivery.Id);
                    cartDeliveries.Add(new Tuple<DeliveryMode, DateTimeOffset, string>(delivery, cartDelivery.ExpectedDeliveryDate, cartDelivery.Comment));
                }

                var user = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var entity = await _context.GetByIdAsync<Order>(request.Id, token);

                entity.SetProducts(cartProducts);
                entity.SetDeliveries(cartDeliveries);
                entity.SetDonation(request.Donation);
                entity.RefreshFees(_pspOptions.FixedAmount, _pspOptions.Percent);

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<Guid>> Handle(PayOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var checkResult = await _mediatr.Send(new EnsureConsumerConfiguredCommand(request.RequestUser) { Id = request.RequestUser.Id }, token);
                if (!checkResult.Success)
                    return Failed<Guid>(checkResult.Exception);

                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var order = await _context.GetByIdAsync<Order>(request.Id, token);
                    order.SetStatus(OrderStatusKind.Waiting);

                    _context.Update(order);
                    await _context.SaveChangesAsync(token);

                    var result = await _mediatr.Send(new CreateWebPayInTransactionCommand(request.RequestUser) { OrderId = order.Id }, token);
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

                    order.SetStatus(OrderStatusKind.Validated);

                    var producerIds = order.Products.Select(p => p.Producer.Id).Distinct();
                    foreach(var producerId in producerIds)
                    {
                        var result = await _mediatr.Send(new CreatePurchaseOrderCommand(request.RequestUser)
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
                        await _queuesService.ProcessEventAsync(PurchaseOrderCreatedEvent.QUEUE_NAME, new PurchaseOrderCreatedEvent(request.RequestUser) { Id = orderId }, token);

                    await _queuesService.ProcessCommandAsync(CreateUserPointsCommand.QUEUE_NAME, new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = request.RequestUser.Id }, token);

                    return Ok(orderIds.AsEnumerable());
                }
            });
        }
    }
}
