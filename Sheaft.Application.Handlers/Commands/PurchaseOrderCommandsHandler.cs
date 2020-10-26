using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Sheaft.Application.Events;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain.Enums;

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
        public PurchaseOrderCommandsHandler(
            IAppDbContext context,
            ISheaftMediatr mediatr,
            ILogger<PurchaseOrderCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreatePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var producer = await _context.GetByIdAsync<Producer>(request.ProducerId, token);
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);

                var resultIdentifier = await _mediatr.Process(new CreatePurchaseOrderIdentifierCommand(request.RequestUser) { ProducerId = request.ProducerId }, token);
                if (!resultIdentifier.Success)
                    return Failed<Guid>(resultIdentifier.Exception);

                var purchaseOrderId = order.AddPurchaseOrder(resultIdentifier.Data, producer);
                await _context.SaveChangesAsync(token);

                if (!request.SkipSendEmail)
                {
                    _mediatr.Post(new PurchaseOrderCreatedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrderId });
                    _mediatr.Post(new PurchaseOrderReceivedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrderId });
                }

                return Ok(purchaseOrderId);
            });
        }

        public async Task<Result<bool>> Handle(ShipPurchaseOrdersCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
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
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
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
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
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
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
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
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
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
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
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
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
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
            return await ExecuteAsync(request, async () =>
            {
                using (var transaction = await _context.BeginTransactionAsync(token))
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
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Ship();

                await _context.SaveChangesAsync(token);
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(DeliverPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Deliver();

                await _context.SaveChangesAsync(token);

                _mediatr.Schedule(new CreateTransferCommand(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id }, TimeSpan.FromDays(7));
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CancelPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Cancel(request.Reason);

                await _context.SaveChangesAsync(token);

                if (request.RequestUser.Id == purchaseOrder.Sender.Id)
                    _mediatr.Post(new PurchaseOrderWithdrawnEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });
                else
                    _mediatr.Post(new PurchaseOrderCancelledEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RefusePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Refuse(request.Reason);

                await _context.SaveChangesAsync(token);

                _mediatr.Post(new PurchaseOrderRefusedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(ProcessPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Process();

                await _context.SaveChangesAsync(token);

                _mediatr.Post(new PurchaseOrderProcessingEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(CompletePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Complete();

                await _context.SaveChangesAsync(token);

                _mediatr.Post(new PurchaseOrderCompletedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(AcceptPurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);
                purchaseOrder.Accept();

                await _context.SaveChangesAsync(token);
                
                _mediatr.Post(new PurchaseOrderAcceptedEvent(request.RequestUser) { PurchaseOrderId = purchaseOrder.Id });
                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(DeletePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.GetByIdAsync<PurchaseOrder>(request.Id, token);

                _context.Remove(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(RestorePurchaseOrderCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var entity = await _context.PurchaseOrders.SingleOrDefaultAsync(a => a.Id == request.Id && a.RemovedOn.HasValue, token);

                _context.Restore(entity);
                await _context.SaveChangesAsync(token);

                return Ok(true);
            });
        }
    }
}
