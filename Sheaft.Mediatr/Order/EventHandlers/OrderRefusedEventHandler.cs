using System.Threading;
using System.Threading.Tasks;
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
    public class OrderRefusedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<OrderRefusedEvent>>
    {
        private readonly IConfiguration _configuration;

        public OrderRefusedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            IConfiguration configuration,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<OrderRefusedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var order = await _context.Orders.SingleAsync(e => e.Id == orderEvent.OrderId, token);
            
            await _signalrService.SendNotificationToUserAsync(order.Id, nameof(OrderRefusedEvent),
                order.GetOrderNotifModelAsString());

            await _emailService.SendTemplatedEmailAsync(
                order.User.Email,
                order.User.Name,
                $"Votre commande de {order.TotalPrice}€ est refusée",
                nameof(OrderRefusedEvent),
                order.GetTemplateData(
                    $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{order.Id}"),
                true,
                token);
        }
    }
}