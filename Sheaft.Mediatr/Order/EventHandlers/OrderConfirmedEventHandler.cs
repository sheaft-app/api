using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Order;
using Sheaft.Mailing.Extensions;

namespace Sheaft.Mediatr.Order.EventHandlers
{
    public class OrderConfirmedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<OrderConfirmedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public OrderConfirmedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            IConfiguration configuration,
            IIdSerializer idSerializer,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<OrderConfirmedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var order = await _context.Orders.SingleAsync(e => e.Id == orderEvent.OrderId, token);
            
            var purchaseOrderId = order.PurchaseOrders.Count() == 1 ? order.PurchaseOrders.FirstOrDefault()?.Id : (Guid?)null; 
            var purchaseOrderIdentifier = purchaseOrderId.HasValue ? _idSerializer.Serialize("Query", nameof(PurchaseOrder), purchaseOrderId): string.Empty;
            await _signalrService.SendNotificationToUserAsync(order.UserId.Value, nameof(OrderConfirmedEvent),
                order.GetOrderNotifModelAsString(purchaseOrderIdentifier));

            await _emailService.SendTemplatedEmailAsync(
                order.User.Email,
                order.User.Name,
                $"Votre commande de {order.TotalPrice}€ a été prise en compte",
                nameof(OrderConfirmedEvent),
                order.GetTemplateData(_idSerializer.Serialize("Query", nameof(Order), orderEvent.OrderId),
                    $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrderIdentifier}"),
                true,
                token);
        }
    }
}