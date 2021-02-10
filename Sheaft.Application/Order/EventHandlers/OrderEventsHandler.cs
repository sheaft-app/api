using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Commands.Handlers;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
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
            var order = await _context.GetByIdAsync<Order>(orderEvent.OrderId, token);

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