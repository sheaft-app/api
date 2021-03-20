using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Donation;

namespace Sheaft.Mediatr.Donation.EventHandlers
{
    public class DonationFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DonationFailedEvent>>
    {
        public DonationFailedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<DonationFailedEvent> notification, CancellationToken token)
        {
            var donationEvent = notification.DomainEvent;
            var donation = await _context.GetByIdAsync<Domain.Donation>(donationEvent.DonationId, token);
            if (donation.Status != TransactionStatus.Failed)
                return;

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La donation de {donation.Debited}€ {donation.Author.Name} a échouée",
               $"La donation de {donation.Debited}€ pour le consommateur {donation.Author.Name} ({donation.Author.Email}) a échouée. Raison: {donation.ResultCode}-{donation.ResultMessage}.",
               false,
               token);
        }
    }
}