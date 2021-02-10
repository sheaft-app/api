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
            var donation = await _context.GetByIdAsync<Donation>(donationEvent.DonationId, token);
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