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
    public class CreateDeliveryBatchCommand : Command<Guid>
    {
        protected CreateDeliveryBatchCommand()
        {
        }
        
        [JsonConstructor]
        public CreateDeliveryBatchCommand(RequestUser requestUser) : base(requestUser)
        {
            ProducerId = RequestUser.Id;
        }

        public string Name { get; set; }
        public DateTimeOffset ScheduledOn { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public Guid ProducerId { get; set; }
        public IEnumerable<PurchaseOrderDeliveryPositionDto> Deliveries { get; set; }

        public override void SetRequestUser(RequestUser user)
        {
            base.SetRequestUser(user);
            ProducerId = RequestUser.Id;
        }
    }

    public class CreateDeliveryBatchCommandHandler : CommandsHandler,
        IRequestHandler<CreateDeliveryBatchCommand, Result<Guid>>
    {
        public CreateDeliveryBatchCommandHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<CreateDeliveryBatchCommandHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<Guid>> Handle(CreateDeliveryBatchCommand request, CancellationToken token)
        {
            var deliveryIds = request.Deliveries.Select(d => d.DeliveryId);
            var purchaseOrders = await _context.PurchaseOrders
                .Where(po => deliveryIds.Contains(po.Delivery.Id))
                .Include(po => po.Delivery)
                .ThenInclude(d => d.DeliveryMode)
                .ToListAsync(token);

            if (purchaseOrders.Any(po => po.Delivery.Status == DeliveryStatus.Delivered))
                return Failure<Guid>(MessageKind.BadRequest);

            var name = request.Name;
            if (string.IsNullOrWhiteSpace(name) && purchaseOrders.Any())
            {
                var deliveryMode = purchaseOrders
                    .Select(po => po.Delivery.DeliveryMode)
                    .GroupBy(p => p.Id)
                    .OrderByDescending(e => e.Count())
                    .First();

                name = deliveryMode.First().Name;
            }
            
            var user = await _context.Users.SingleAsync(u => u.Id == request.ProducerId, token);
            var deliveryBatch = new DeliveryBatch(Guid.NewGuid(), name, request.ScheduledOn, request.From, request.To, user,
                purchaseOrders.Select(po => po.Delivery).ToList());

            await _context.AddAsync(deliveryBatch, token);
            await _context.SaveChangesAsync(token);

            return Success(deliveryBatch.Id);
        }
    }
}