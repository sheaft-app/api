using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop.Enums;
using Sheaft.Application.Events;
using Microsoft.Extensions.Configuration;
using Sheaft.Services.Interop;
using Sheaft.Models.Inputs;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class PurchaseOrderCommandsHandler : CommandsHandler,
        IRequestHandler<CreatePurchaseOrdersCommand, Result<IEnumerable<Guid>>>,
        IRequestHandler<CreatePurchaseOrderCommand, Result<Guid>>,
        IRequestHandler<UpdatePurchaseOrderProductsCommand, Result<bool>>,

        IRequestHandler<AcceptPurchaseOrdersCommand, Result<bool>>,
        IRequestHandler<ProcessPurchaseOrdersCommand, Result<bool>>,
        IRequestHandler<CancelPurchaseOrdersCommand, Result<bool>>,
        IRequestHandler<RefusePurchaseOrdersCommand, Result<bool>>,
        IRequestHandler<CompletePurchaseOrdersCommand, Result<bool>>,
        IRequestHandler<ShipPurchaseOrdersCommand, Result<bool>>,
        IRequestHandler<DeliverPurchaseOrdersCommand, Result<bool>>,
        IRequestHandler<DeletePurchaseOrdersCommand, Result<bool>>,

        IRequestHandler<AcceptPurchaseOrderCommand, Result<bool>>,
        IRequestHandler<ProcessPurchaseOrderCommand, Result<bool>>,
        IRequestHandler<CancelPurchaseOrderCommand, Result<bool>>,
        IRequestHandler<RefusePurchaseOrderCommand, Result<bool>>,
        IRequestHandler<CompletePurchaseOrderCommand, Result<bool>>,
        IRequestHandler<ShipPurchaseOrderCommand, Result<bool>>,
        IRequestHandler<DeliverPurchaseOrderCommand, Result<bool>>,
        IRequestHandler<DeletePurchaseOrderCommand, Result<bool>>,
        IRequestHandler<RestorePurchaseOrderCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediatr;
        private readonly IIdentifierService _identifierService;
        private readonly IQueueService _queuesService;

        public PurchaseOrderCommandsHandler(
            IAppDbContext context,
            IIdentifierService identifierService,
            IMediator mediatr,
            IQueueService queuesService,
            ILogger<PurchaseOrderCommandsHandler> logger) : base(logger)
        {
            _context = context;
            _mediatr = mediatr;
            _identifierService = identifierService;
            _queuesService = queuesService;
        }

        public async Task<Result<IEnumerable<Guid>>> Handle(CreatePurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var productIds = request.Products.Select(p => p.Id);
                    var products = await _context.GetByIdsAsync<Product>(productIds, token);

                    var orderResult = await _mediatr.Send(new CreateOrderCommand(request.RequestUser) { Donation = request.Donation ?? 0 }, token);
                    if (!orderResult.Success)
                        return Failed<IEnumerable<Guid>>(orderResult.Exception);

                    var createdOrders = new List<Guid>();
                    foreach (var producerId in products.GroupBy(c => c.Producer.Id).Select(c => c.Key))
                    {
                        var cartProducts = products.Where(p => p.Producer.Id == producerId)
                            .Select(c => new ProductQuantityInput { Id = c.Id, Quantity = request.Products.Where(p => p.Id == c.Id).Sum(c => c.Quantity) });

                        var expectedDelivery = request.ProducersExpectedDeliveries.SingleOrDefault(c => c.ProducerId == producerId);
                        var result = await _mediatr.Send(new CreatePurchaseOrderCommand(request.RequestUser) { OrderId = orderResult.Data, SkipSendEmail = true, ProducerId = producerId, Comment = expectedDelivery.Comment, ExpectedDeliveryDate = expectedDelivery.ExpectedDeliveryDate, DeliveryModeId = expectedDelivery.DeliveryModeId, Products = cartProducts }, token);
                        if (!result.Success)
                            return Failed<IEnumerable<Guid>>(result.Exception);

                        createdOrders.Add(result.Data);
                    }

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    foreach (var orderId in createdOrders)
                        await _queuesService.ProcessEventAsync(PurchaseOrderCreatedEvent.QUEUE_NAME, new PurchaseOrderCreatedEvent(request.RequestUser) { Id = orderId }, token);

                    await _queuesService.ProcessCommandAsync(CreateUserPointsCommand.QUEUE_NAME, new CreateUserPointsCommand(request.RequestUser) { CreatedOn = DateTimeOffset.UtcNow, Kind = PointKind.PurchaseOrder, UserId = request.RequestUser.Id }, token);

                    return Ok(createdOrders.AsEnumerable());
                }
            });
        }

        public async Task<Result<Guid>> Handle(CreatePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var sender = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var producer = await _context.GetByIdAsync<Producer>(request.ProducerId, token);
                var deliveryMode = await _context.GetByIdAsync<DeliveryMode>(request.DeliveryModeId, token);
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);

                var productIds = request.Products.Select(p => p.Id);
                var products = await _context.GetByIdsAsync<Product>(productIds, token);

                var cartProducts = products
                            .Select(c => new { Product = c, Quantity = request.Products.Where(p => p.Id == c.Id).Sum(c => c.Quantity) })
                            .ToDictionary(d => d.Product, d => d.Quantity);

                var result = await _identifierService.GetNextOrderReferenceAsync(request.ProducerId, token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                var entity = new PurchaseOrder(Guid.NewGuid(), result.Data, OrderStatusKind.Waiting, cartProducts, deliveryMode, request.ExpectedDeliveryDate, producer, sender);
                entity.SetComment(request.Comment);

                order.AddPurchaseOrder(entity);

                await _context.SaveChangesAsync(token);

                if (!request.SkipSendEmail)
                    await _queuesService.ProcessEventAsync(PurchaseOrderCreatedEvent.QUEUE_NAME, new PurchaseOrderCreatedEvent(request.RequestUser) { Id = entity.Id }, token);

                return Ok(entity.Id);
            });
        }

        public async Task<Result<bool>> Handle(UpdatePurchaseOrderProductsCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                var products = request.Products.ToList();
                foreach (var orderProduct in entity.Products.ToList())
                {
                    var productToUpdate = request.Products.SingleOrDefault(p => p.Id == orderProduct.Id);
                    if (productToUpdate == null || productToUpdate.Quantity == 0)
                        entity.RemoveProduct(orderProduct.Id);
                    else
                    {
                        entity.ChangeProductQuantity(orderProduct.Id, productToUpdate.Quantity);
                        products.Remove(productToUpdate);
                    }
                }

                foreach (var newProduct in products)
                {
                    var product = await _context.FindByIdAsync<Product>(newProduct.Id, token);
                    entity.AddProduct(product, newProduct.Quantity);
                }

                _context.Update(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(ShipPurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var purchaseOrderId in request.Ids)
                    {
                        var result = await _mediatr.Send(new ShipPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(DeliverPurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var purchaseOrderId in request.Ids)
                    {
                        var result = await _mediatr.Send(new DeliverPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(CancelPurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var purchaseOrderId in request.Ids)
                    {
                        var result = await _mediatr.Send(new CancelPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId, Reason = request.Reason }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(RefusePurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var errors = new List<string>();
                    foreach (var purchaseOrderId in request.Ids)
                    {
                        var result = await _mediatr.Send(new RefusePurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId, Reason = request.Reason }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(ProcessPurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var purchaseOrderId in request.Ids)
                    {
                        var result = await _mediatr.Send(new ProcessPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(CompletePurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var purchaseOrderId in request.Ids)
                    {
                        var result = await _mediatr.Send(new CompletePurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(AcceptPurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var purchaseOrderId in request.Ids)
                    {
                        var result = await _mediatr.Send(new AcceptPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(DeletePurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    foreach (var purchaseOrderId in request.Ids)
                    {
                        var result = await _mediatr.Send(new DeletePurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
                        if (!result.Success)
                            return Failed<bool>(result.Exception);
                    }

                    await transaction.CommitAsync(token);
                    return Ok(true);
                }
            });
        }

        public async Task<Result<bool>> Handle(ShipPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                purchaseOrder.Ship();
                _context.Update(purchaseOrder);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(DeliverPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                purchaseOrder.Deliver();
                _context.Update(purchaseOrder);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(CancelPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                purchaseOrder.Cancel(request.Reason);
                _context.Update(purchaseOrder);

                if (request.RequestUser.Id == purchaseOrder.Sender.Id)
                    await _queuesService.ProcessEventAsync(PurchaseOrderCancelledBySenderEvent.QUEUE_NAME, new PurchaseOrderCancelledBySenderEvent(request.RequestUser) { Id = purchaseOrder.Id }, token);
                else
                    await _queuesService.ProcessEventAsync(PurchaseOrderCancelledByVendorEvent.QUEUE_NAME, new PurchaseOrderCancelledByVendorEvent(request.RequestUser) { Id = purchaseOrder.Id }, token);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(RefusePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                purchaseOrder.Refuse(request.Reason);
                _context.Update(purchaseOrder);

                await _queuesService.ProcessEventAsync(PurchaseOrderRefusedEvent.QUEUE_NAME, new PurchaseOrderRefusedEvent(request.RequestUser) { Id = purchaseOrder.Id }, token);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(ProcessPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                purchaseOrder.Process();
                _context.Update(purchaseOrder);

                await _queuesService.ProcessEventAsync(PurchaseOrderProcessingEvent.QUEUE_NAME, new PurchaseOrderProcessingEvent(request.RequestUser) { Id = purchaseOrder.Id }, token);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(CompletePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                purchaseOrder.Complete();
                _context.Update(purchaseOrder);

                await _queuesService.ProcessEventAsync(PurchaseOrderCompletedEvent.QUEUE_NAME, new PurchaseOrderCompletedEvent(request.RequestUser) { Id = purchaseOrder.Id }, token);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(AcceptPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                purchaseOrder.Accept();
                _context.Update(purchaseOrder);

                await _queuesService.ProcessEventAsync(PurchaseOrderAcceptedEvent.QUEUE_NAME, new PurchaseOrderAcceptedEvent(request.RequestUser) { Id = purchaseOrder.Id }, token);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(DeletePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                _context.Remove(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<Result<bool>> Handle(RestorePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.PurchaseOrders.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                _context.Restore(entity);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}
