using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Core.Enums;

namespace Sheaft.Mediatr.DeliveryBatch.Commands
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
        public Guid ProducerId { get; set; }
        public IEnumerable<ClientDeliveryPositionDto> Deliveries { get; set; }

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
            var purchaseOrders = await GetPurchaseOrdersAsync(request, token);
            if (purchaseOrders.Any(po => po.Status == PurchaseOrderStatus.Delivered))
                return Failure<Guid>(MessageKind.Validation);

            if (purchaseOrders.Any(po => (int) po.ExpectedDelivery.Kind <= 4))
                return Failure<Guid>(MessageKind.Validation);

            var name = GetDeliveryBatchName(request, purchaseOrders);
            var producer = await _context.Producers.SingleAsync(p => p.Id == request.ProducerId, token);
            var usersAgreements = await GetUsersPositionsAsync(request, token, purchaseOrders);

            var purchaseOrderIds = request.Deliveries.SelectMany(d => d.PurchaseOrderIds);
            var purchaseOrdersDeliveries =
                await _context.Deliveries.Where(d => d.PurchaseOrders.Any(po => purchaseOrderIds.Contains(po.Id)))
                    .Include(d => d.DeliveryBatch)
                    .Include(d => d.PurchaseOrders)
                    .ToListAsync(token);

            var deliveries = new List<Domain.Delivery>();
            foreach (var delivery in purchaseOrdersDeliveries)
            {
                var assignedPurchaseOrders = delivery.PurchaseOrders
                    .Where(po => purchaseOrderIds.Contains(po.Id))
                    .ToList();

                if (delivery.PurchaseOrders.Count == assignedPurchaseOrders.Count)
                {
                    if (delivery.DeliveryBatch != null)
                    {
                        if (delivery.DeliveryBatch.ScheduledOn != request.ScheduledOn)
                            delivery.PostponeDelivery();

                        delivery.DeliveryBatch.RemoveDelivery(delivery);
                    }

                    deliveries.Add(delivery);

                    foreach (var assignedPurchaseOrder in assignedPurchaseOrders)
                    {
                        var pendingPurchaseOrder = purchaseOrders.Single(po => po.Id == assignedPurchaseOrder.Id);
                        purchaseOrders.Remove(pendingPurchaseOrder);
                    }
                }
                else
                {
                    delivery.RemovePurchaseOrders(assignedPurchaseOrders);

                    var newDelivery = new Domain.Delivery(producer, delivery.Kind, request.ScheduledOn,
                        delivery.Address,
                        delivery.ClientId, delivery.Client, assignedPurchaseOrders, delivery.Position);

                    if (delivery.DeliveryBatch != null && delivery.DeliveryBatch.ScheduledOn != request.ScheduledOn)
                        newDelivery.PostponeDelivery();

                    deliveries.Add(newDelivery);
                }
            }

            foreach (var purchaseOrderGrouped in purchaseOrders
                .GroupBy(p => p.ClientId)
                .OrderByDescending(e => e.Count())
                .ToList())
            {
                var order = purchaseOrderGrouped.First();
                var existingDelivery = deliveries.SingleOrDefault(d => d.ClientId == order.ClientId);
                if (existingDelivery != null)
                {
                    if (existingDelivery.PurchaseOrders.Any(po => po.Id == order.Id))
                        continue;

                    existingDelivery.AddPurchaseOrders(new[] {order});
                }
                else
                {
                    var userAgreement = usersAgreements.First(a => a.User.Id == order.ClientId);
                    var position = userAgreement?.Position ?? 0;
                    while (deliveries.Any(d => d.Position == position))
                        position++;

                    var delivery = new Domain.Delivery(producer, order.ExpectedDelivery.Kind, request.ScheduledOn,
                        order.ExpectedDelivery.Address, userAgreement.User.Id, userAgreement.User.Name,
                        purchaseOrderGrouped.ToList(), position);

                    deliveries.Add(delivery);
                }
            }

            var deliveryBatch = new Domain.DeliveryBatch(Guid.NewGuid(), name, request.ScheduledOn, request.From,
                producer, deliveries);

            await _context.AddAsync(deliveryBatch, token);
            await _context.SaveChangesAsync(token);

            return Success(deliveryBatch.Id);
        }

        private async Task<IEnumerable<UserAgreementPosition>> GetUsersPositionsAsync(
            CreateDeliveryBatchCommand request, CancellationToken token, List<Domain.PurchaseOrder> purchaseOrders)
        {
            var userIds = purchaseOrders.Select(po => po.ClientId);
            var agreements = await _context.Agreements.Where(a =>
                    userIds.Contains(a.StoreId) && a.Status == AgreementStatus.Accepted &&
                    a.ProducerId == request.ProducerId)
                .ToListAsync(token);

            var users = await _context.Users
                .Where(p => userIds.Contains(p.Id))
                .ToListAsync(token);

            return users.Select(u => new UserAgreementPosition
            {
                User = u,
                Position = agreements.FirstOrDefault(a => a.StoreId == u.Id)?.Position
            });
        }

        private async Task<List<Domain.PurchaseOrder>> GetPurchaseOrdersAsync(CreateDeliveryBatchCommand request,
            CancellationToken token)
        {
            var purchaseOrderIds = request.Deliveries.SelectMany(d => d.PurchaseOrderIds);
            var purchaseOrders = await _context.PurchaseOrders
                .Where(po => purchaseOrderIds.Contains(po.Id))
                .ToListAsync(token);
            return purchaseOrders;
        }

        private static string GetDeliveryBatchName(CreateDeliveryBatchCommand request,
            List<Domain.PurchaseOrder> purchaseOrders)
        {
            var name = request.Name;
            if (string.IsNullOrWhiteSpace(name))
            {
                name = purchaseOrders
                    .GroupBy(p => p.ExpectedDelivery.Name)
                    .OrderByDescending(e => e.Count())
                    .First()
                    .First()
                    .ExpectedDelivery.Name;
            }

            return name;
        }

        private class UserAgreementPosition
        {
            public int? Position { get; set; }
            public Domain.User User { get; set; }
        }
    }
}