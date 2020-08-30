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
        IRequestHandler<CreatePurchaseOrdersCommand, CommandResult<IEnumerable<Guid>>>,
        IRequestHandler<CreatePurchaseOrderCommand, CommandResult<Guid>>,
        IRequestHandler<UpdatePurchaseOrderProductsCommand, CommandResult<bool>>,

        IRequestHandler<AcceptPurchaseOrdersCommand, CommandResult<bool>>,
        IRequestHandler<ProcessPurchaseOrdersCommand, CommandResult<bool>>,
        IRequestHandler<CancelPurchaseOrdersCommand, CommandResult<bool>>,
        IRequestHandler<RefusePurchaseOrdersCommand, CommandResult<bool>>,
        IRequestHandler<CompletePurchaseOrdersCommand, CommandResult<bool>>,
        IRequestHandler<ShipPurchaseOrdersCommand, CommandResult<bool>>,
        IRequestHandler<DeliverPurchaseOrdersCommand, CommandResult<bool>>,
        IRequestHandler<DeletePurchaseOrdersCommand, CommandResult<bool>>,

        IRequestHandler<AcceptPurchaseOrderCommand, CommandResult<bool>>,
        IRequestHandler<ProcessPurchaseOrderCommand, CommandResult<bool>>,
        IRequestHandler<CancelPurchaseOrderCommand, CommandResult<bool>>,
        IRequestHandler<RefusePurchaseOrderCommand, CommandResult<bool>>,
        IRequestHandler<CompletePurchaseOrderCommand, CommandResult<bool>>,
        IRequestHandler<ShipPurchaseOrderCommand, CommandResult<bool>>,
        IRequestHandler<DeliverPurchaseOrderCommand, CommandResult<bool>>,
        IRequestHandler<DeletePurchaseOrderCommand, CommandResult<bool>>,
        IRequestHandler<RestorePurchaseOrderCommand, CommandResult<bool>>
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

        public async Task<CommandResult<IEnumerable<Guid>>> Handle(CreatePurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(token))
                {
                    var productIds = request.Products.Select(p => p.Id);
                    var products = await _context.GetByIdsAsync<Product>(productIds, token);

                    var createdOrders = new List<Guid>();
                    foreach (var companyId in products.GroupBy(c => c.Producer.Id).Select(c => c.Key))
                    {
                        var cartProducts = products.Where(p => p.Producer.Id == companyId)
                            .Select(c => new ProductQuantityInput { Id = c.Id, Quantity = request.Products.Where(p => p.Id == c.Id).Sum(c => c.Quantity) });

                        var expectedDelivery = request.ProducersExpectedDeliveries.SingleOrDefault(c => c.ProducerId == companyId);
                        var result = await _mediatr.Send(new CreatePurchaseOrderCommand(request.RequestUser) { SkipSendEmail = true, ProducerId = companyId, Comment = expectedDelivery.Comment, ExpectedDeliveryDate = expectedDelivery.ExpectedDeliveryDate, DeliveryModeId = expectedDelivery.DeliveryModeId, Products = cartProducts }, token);
                        if (!result.Success)
                            return Failed<IEnumerable<Guid>>(result.Exception);

                        createdOrders.Add(result.Result);
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

        public async Task<CommandResult<Guid>> Handle(CreatePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var sender = await _context.GetByIdAsync<User>(request.RequestUser.Id, token);
                var company = await _context.GetByIdAsync<Company>(request.ProducerId, token);
                var deliveryMode = await _context.GetByIdAsync<DeliveryMode>(request.DeliveryModeId, token);

                var productIds = request.Products.Select(p => p.Id);
                var products = await _context.GetByIdsAsync<Product>(productIds, token);

                var cartProducts = products
                            .Select(c => new { Product = c, Quantity = request.Products.Where(p => p.Id == c.Id).Sum(c => c.Quantity) })
                            .ToDictionary(d => d.Product, d => d.Quantity);

                var result = await _identifierService.GetNextOrderReferenceAsync(request.ProducerId, token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                var entity = new PurchaseOrder(Guid.NewGuid(), result.Result, OrderStatusKind.Waiting, cartProducts, deliveryMode, request.ExpectedDeliveryDate, company, sender);
                entity.SetComment(request.Comment);

                await _context.AddAsync(entity, token);
                await _context.SaveChangesAsync(token);

                if (!request.SkipSendEmail)
                    await _queuesService.ProcessEventAsync(PurchaseOrderCreatedEvent.QUEUE_NAME, new PurchaseOrderCreatedEvent(request.RequestUser) { Id = entity.Id }, token);

                return Ok(entity.Id);
            });
        }

        public async Task<CommandResult<bool>> Handle(UpdatePurchaseOrderProductsCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(ShipPurchaseOrdersCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(DeliverPurchaseOrdersCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(CancelPurchaseOrdersCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(RefusePurchaseOrdersCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(ProcessPurchaseOrdersCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(CompletePurchaseOrdersCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(AcceptPurchaseOrdersCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(DeletePurchaseOrdersCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(ShipPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                purchaseOrder.Ship();
                _context.Update(purchaseOrder);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(DeliverPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                purchaseOrder.Deliver();
                _context.Update(purchaseOrder);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(CancelPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                purchaseOrder.Cancel(request.Reason);
                _context.Update(purchaseOrder);

                if (request.RequestUser.Id == purchaseOrder.Sender.Id || request.RequestUser.CompanyId == purchaseOrder.Sender.Id)
                    await _queuesService.ProcessEventAsync(PurchaseOrderCancelledBySenderEvent.QUEUE_NAME, new PurchaseOrderCancelledBySenderEvent(request.RequestUser) { Id = purchaseOrder.Id }, token);
                else
                    await _queuesService.ProcessEventAsync(PurchaseOrderCancelledByVendorEvent.QUEUE_NAME, new PurchaseOrderCancelledByVendorEvent(request.RequestUser) { Id = purchaseOrder.Id }, token);

                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(RefusePurchaseOrderCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(ProcessPurchaseOrderCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(CompletePurchaseOrderCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(AcceptPurchaseOrderCommand request, CancellationToken token)
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

        public async Task<CommandResult<bool>> Handle(DeletePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                entity.Remove();

                _context.Remove(entity);
                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }

        public async Task<CommandResult<bool>> Handle(RestorePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _context.PurchaseOrders.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);
                entity.Restore();

                _context.Update(entity);
                return Ok(await _context.SaveChangesAsync(token) > 0);
            });
        }
    }
}
