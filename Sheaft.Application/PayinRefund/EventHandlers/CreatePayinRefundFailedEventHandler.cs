using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Events.PayinRefund;

namespace Sheaft.Application.PayinRefund.EventHandlers
{
    public class CreatePayinRefundFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<CreateOrderPayinRefundFailedEvent>>
    {
        public CreatePayinRefundFailedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<CreateOrderPayinRefundFailedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var order = await _context.GetByIdAsync<Domain.Order>(orderEvent.OrderId, token);

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