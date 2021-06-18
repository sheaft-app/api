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
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.DeliveryMode.Commands
{
    public class UpdateDeliveryBatchCommand : Command
    {
        protected UpdateDeliveryBatchCommand()
        {
        }
        
        [JsonConstructor]
        public UpdateDeliveryBatchCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PurchaseOrderDeliveryPositionDto> Deliveries { get; set; }
    }

    public class UpdateDeliveryBatchCommandHandler : CommandsHandler,
        IRequestHandler<UpdateDeliveryBatchCommand, Result>
    {
        public UpdateDeliveryBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<UpdateDeliveryBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result> Handle(UpdateDeliveryBatchCommand request, CancellationToken token)
        {
            var deliveryIds = request.Deliveries.Select(d => d.DeliveryId);
            var purchaseOrders = await _context.PurchaseOrders
                .Where(po => deliveryIds.Contains(po.Delivery.Id))
                .Include(po => po.Delivery)
                .ToListAsync(token);

            if (purchaseOrders.Any(po => po.Delivery.Status == DeliveryStatus.Delivered))
                return Failure<Guid>(MessageKind.BadRequest);

            var deliveryBatch = await _context.DeliveryBatches.SingleOrDefaultAsync(c => c.Id == request.Id, token);
            if (deliveryBatch == null)
                return Failure(MessageKind.NotFound);

            deliveryBatch.SetDeliveries(purchaseOrders.Select(po => po.Delivery).ToList());
            deliveryBatch.SetName(request.Name);
            
            await _context.SaveChangesAsync(token);
            return Success();
        }
    }
}