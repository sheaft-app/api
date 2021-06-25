using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Core.Enums;
using Sheaft.Domain.Events.DeliveryBatch;
using Sheaft.Mediatr.Producer.Commands;

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

        public Guid Id { get; set; }
        public DateTimeOffset? ReschedulePendingDeliveriesOn { get; set; }
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
            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.Id, token);
            if (deliveryBatch == null)
                return Failure(MessageKind.NotFound);

            var pendingDeliveries = deliveryBatch.Deliveries
                .Where(d => d.Status != DeliveryStatus.Delivered && d.Status != DeliveryStatus.Rejected)
                .ToList();

            if (pendingDeliveries.Any() && !request.ReschedulePendingDeliveriesOn.HasValue)
                return Failure(MessageKind.Validation);

            if (pendingDeliveries.Any())
            {
                var result = await _mediatr.Process(new CreateDeliveryBatchCommand(request.RequestUser)
                {
                    Deliveries = pendingDeliveries.Select(p => new ClientDeliveryPositionDto
                    {
                        Position = p.Position,
                        ClientId = p.ClientId,
                        PurchaseOrderIds = p.PurchaseOrders.Select(po => po.Id)
                    }),
                    From = request.ReschedulePendingDeliveriesOn.Value.TimeOfDay,
                    Name = $"{deliveryBatch.Name} - replanifiée",
                    ScheduledOn = request.ReschedulePendingDeliveriesOn.Value
                }, token);

                if (!result.Succeeded)
                    return Failure(result);

                _mediatr.Post(new DeliveryBatchPostponedEvent(result.Data));
            }

            deliveryBatch.CompleteBatch();
            await _context.SaveChangesAsync(token);

            if (!pendingDeliveries.Any())
                _mediatr.Post(new GenerateDeliveryBatchFormsCommand(request.RequestUser)
                    {DeliveryBatchId = deliveryBatch.Id});

            return Success();
        }
    }
}