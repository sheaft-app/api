using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Application.Events;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Sheaft.Application.Handlers
{
    public class PurchaseOrderCommandsHandler : ResultsHandler,
        IRequestHandler<CreatePurchaseOrderCommand, Result<Guid>>,
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
        private readonly IIdentifierService _identifierService;

        public PurchaseOrderCommandsHandler(
            IAppDbContext context,
            IIdentifierService identifierService,
            ISheaftMediatr mediatr,
            ILogger<PurchaseOrderCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
            _identifierService = identifierService;
        }

        public async Task<Result<Guid>> Handle(CreatePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.ProducerId, token);
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);

                var result = await _identifierService.GetNextOrderReferenceAsync(request.ProducerId, token);
                if (!result.Success)
                    return Failed<Guid>(result.Exception);

                var entity = new PurchaseOrder(Guid.NewGuid(), result.Data, PurchaseOrderStatus.Waiting, producer, order);
                await _context.SaveChangesAsync(token);

                if (!request.SkipSendEmail)
                    await _mediatr.Post(new PurchaseOrderCreatedEvent(request.RequestUser) { PurchaseOrderId = entity.Id }, token);

                return Ok(entity.Id);
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
                        var result = await _mediatr.Process(new ShipPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
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
                        var result = await _mediatr.Process(new DeliverPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
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
                        var result = await _mediatr.Process(new CancelPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId, Reason = request.Reason }, token);
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
                        var result = await _mediatr.Process(new RefusePurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId, Reason = request.Reason }, token);
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
                        var result = await _mediatr.Process(new ProcessPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
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
                        var result = await _mediatr.Process(new CompletePurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
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
                        var result = await _mediatr.Process(new AcceptPurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
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
                        var result = await _mediatr.Process(new DeletePurchaseOrderCommand(request.RequestUser) { Id = purchaseOrderId }, token);
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
                    await _mediatr.Post(new PurchaseOrderCancelledBySenderEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id }, token);
                else
                    await _mediatr.Post(new PurchaseOrderCancelledByVendorEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id }, token);

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

                await _mediatr.Post(new PurchaseOrderRefusedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id }, token);

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

                await _mediatr.Post(new PurchaseOrderProcessingEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id }, token);

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

                await _mediatr.Post(new PurchaseOrderCompletedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id }, token);

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

                await _mediatr.Post(new PurchaseOrderAcceptedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id }, token);

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
