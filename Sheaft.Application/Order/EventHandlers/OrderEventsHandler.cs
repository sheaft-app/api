using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Events.Order;

namespace Sheaft.Application.Order.EventHandlers
{
    public class ConfirmOrderFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ConfirmOrderFailedEvent>>
    {
        public ConfirmOrderFailedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<ConfirmOrderFailedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var order = await _context.GetByIdAsync<Domain.Order>(orderEvent.OrderId, token);

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La confirmation de l'order {order.Reference} a échouée",
               $"La confirmation de l'order {order.Reference} d'un montant de {order.TotalPrice}€ pour {order.User.Name} ({order.User.Email}) a échoué. Raison: {orderEvent.Message}.",
               false,
               token);
        }
    }
}