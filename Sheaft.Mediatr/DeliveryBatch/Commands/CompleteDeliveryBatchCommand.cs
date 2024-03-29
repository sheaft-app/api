﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.DeliveryBatch;

namespace Sheaft.Mediatr.DeliveryBatch.Commands
{
    public class CompleteDeliveryBatchCommand : Command
    {
        protected CompleteDeliveryBatchCommand()
        {
        }

        [JsonConstructor]
        public CompleteDeliveryBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeliveryBatchId { get; set; }
        public DateTimeOffset? ReschedulePendingDeliveriesOn { get; set; }
        public TimeSpan? From { get; set; }
    }

    public class CompleteDeliveryBatchCommandHandler : CommandsHandler,
        IRequestHandler<CompleteDeliveryBatchCommand, Result>
    {
        public CompleteDeliveryBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CompleteDeliveryBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(CompleteDeliveryBatchCommand request, CancellationToken token)
        {
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.DeliveryBatchId, token);
            if (deliveryBatch == null)
                return Failure("La tournée de livraison est introuvable.");

            var pendingDeliveries = deliveryBatch.Deliveries
                .Where(d => d.Status != DeliveryStatus.Delivered && d.Status != DeliveryStatus.Rejected)
                .ToList();

            if (pendingDeliveries.Any() && (!request.ReschedulePendingDeliveriesOn.HasValue || !request.From.HasValue))
                return Failure("La tournée de livraison contient encore des livraisons en attente, vous devez spécifier une date de replanification.");

            if (pendingDeliveries.Any())
            {
                var result = await _mediatr.Process(new CreateDeliveryBatchCommand(request.RequestUser)
                {
                    CreatedFromPartialBatchId = deliveryBatch.Id,
                    Deliveries = pendingDeliveries.Select(p => new ClientDeliveryPositionDto
                    {
                        Position = p.Position,
                        ClientId = p.ClientId,
                        PurchaseOrderIds = p.PurchaseOrders.Select(po => po.Id)
                    }),
                    From = request.From.Value,
                    Name = $"{deliveryBatch.Name} - replanifiée",
                    ScheduledOn = request.ReschedulePendingDeliveriesOn.Value
                }, token);

                if (!result.Succeeded)
                    return Failure(result);

                _mediatr.Post(new DeliveryBatchPostponedEvent(result.Data));
            }

            if (!deliveryBatch.Deliveries.Any() || deliveryBatch.Deliveries.Sum(d => d.PurchaseOrdersCount) < 1)
                deliveryBatch.CancelBatch("Livraisons replanifiées");
            else
                deliveryBatch.CompleteBatch(pendingDeliveries.Any());

            await _context.SaveChangesAsync(token);

            if (deliveryBatch.Status is DeliveryBatchStatus.Partial or DeliveryBatchStatus.Completed)
                _mediatr.Post(new GenerateDeliveryBatchFormsCommand(request.RequestUser)
                    {DeliveryBatchId = deliveryBatch.Id});

            return Success();
        }
    }
}