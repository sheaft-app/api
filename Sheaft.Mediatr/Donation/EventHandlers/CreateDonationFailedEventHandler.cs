using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Donation;

namespace Sheaft.Mediatr.Donation.EventHandlers
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
            var order = await _context.GetByIdAsync<Domain.Order>(donationEvent.OrderId, token);

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La création du transfert pour la donation de {order.Donation - order.DonationFeesPrice}€ de {order.User.Name} a échouée",
               $"La création du transfert pour la donation de {order.Donation - order.DonationFeesPrice}€ de {order.User.Name} a échouée à plus de 3 reprises, la donation est mise en supsend, veuillez traiter manuellement la demande.",
               false,
               token);
        }
    }
}