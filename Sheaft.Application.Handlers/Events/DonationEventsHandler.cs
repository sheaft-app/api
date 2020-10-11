using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Options;

namespace Sheaft.Application.Handlers
{
    public class DonationEventsHandler : EventsHandler,
        INotificationHandler<DonationFailedEvent>,
        INotificationHandler<CreateDonationFailedEvent>
    {
        public DonationEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DonationFailedEvent notification, CancellationToken token)
        {
            var donation = await _context.GetByIdAsync<Donation>(notification.DonationId, token);
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

        public async Task Handle(CreateDonationFailedEvent notification, CancellationToken token)
        {
            var order = await _context.GetByIdAsync<Order>(notification.OrderId, token);

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