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
    public class CreateDonationFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<CreateDonationFailedEvent>>
    {
        public CreateDonationFailedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<CreateDonationFailedEvent> notification, CancellationToken token)
        {
            var donationEvent = notification.DomainEvent;
            var order = await _context.GetByIdAsync<Order>(donationEvent.OrderId, token);

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La création du transfert pour la donation de {order.Donate - order.InternalFeesPrice}€ de {order.User.Name} a échouée",
               $"La création du transfert pour la donation de {order.Donate - order.InternalFeesPrice}€ de {order.User.Name} a échouée à plus de 3 reprises, la donation est mise en supsend, veuillez traiter manuellement la demande.",
               false,
               token);
        }
    }
}