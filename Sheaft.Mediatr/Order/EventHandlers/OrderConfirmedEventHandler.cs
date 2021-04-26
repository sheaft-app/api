using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Order;

namespace Sheaft.Mediatr.Order.EventHandlers
{
    public class OrderConfirmedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<OrderConfirmedEvent>>
    {
        private readonly IConfiguration _configuration;

        public OrderConfirmedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            IConfiguration configuration,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<OrderConfirmedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var order = await _context.Orders.SingleAsync(e => e.Id == orderEvent.OrderId, token);
            
            await _signalrService.SendNotificationToUserAsync(order.Id, nameof(OrderConfirmedEvent),
                order.GetOrderNotifModelAsString());

            var orderId = order.PurchaseOrders.Count() == 1 ? order.PurchaseOrders.FirstOrDefault()?.Id : (Guid?)null; 
            
            await _emailService.SendTemplatedEmailAsync(
                order.User.Email,
                order.User.Name,
                $"{order.User.Name}, votre commande de {order.TotalPrice}€ a été prise en compte",
                nameof(OrderConfirmedEvent),
                order.GetTemplateData(
                    $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders{(orderId.HasValue ? $"/{orderId.Value:N}" : string.Empty)}"),
                true,
                token);
        }
    }
}