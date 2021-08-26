using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.Order;
using Sheaft.Mailing.Extensions;

namespace Sheaft.Mediatr.Order.EventHandlers
{
    public class OrderValidatedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<OrderValidatedEvent>>
    {
        private readonly IIdSerializer _idSerializer;
        private readonly IConfiguration _configuration;

        public OrderValidatedEventHandler(
            IAppDbContext context,
            IIdSerializer idSerializer,
            IEmailService emailService,
            IConfiguration configuration,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _idSerializer = idSerializer;
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<OrderValidatedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var order = await _context.Orders.SingleAsync(e => e.Id == orderEvent.OrderId, token);
            if (!order.UserId.HasValue)
                return;
            
            var purchaseOrderId = order.PurchaseOrders.Count() == 1 ? order.PurchaseOrders.FirstOrDefault()?.Id : (Guid?)null; 
            var purchaseOrderIdentifier = purchaseOrderId.HasValue ? _idSerializer.Serialize("Query", nameof(PurchaseOrder), purchaseOrderId): string.Empty;
            await _signalrService.SendNotificationToUserAsync(order.UserId.Value, nameof(OrderValidatedEvent),
                new
                {
                    PurchaseOrderId = order.GetOrderNotifModelAsString(purchaseOrderIdentifier),
                    PortalUrl = $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrderIdentifier}?refresh={Guid.NewGuid():N}",
                });
        }
    }
}