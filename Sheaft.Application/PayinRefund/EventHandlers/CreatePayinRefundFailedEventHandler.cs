using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Commands.Handlers;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class CreatePayinRefundFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<CreatePayinRefundFailedEvent>>
    {
        public CreatePayinRefundFailedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<CreatePayinRefundFailedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var order = await _context.GetByIdAsync<Order>(orderEvent.OrderId, token);

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La création du transfert pour le remboursement du paiement de l'order {order.Reference} pour {order.User.Name} a échouée",
               $"La création du transfert pour le remboursement du paiement de l'order {order.Reference} pour {order.User.Name}({order.User.Email}) a échouée à plus de 3 reprises, le remboursement du paiement est mis en supsend, veuillez traiter manuellement la demande.",
               false,
               token);
        }
    }
}