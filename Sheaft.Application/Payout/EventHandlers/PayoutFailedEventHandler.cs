using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Payout;

namespace Sheaft.Application.Payout.EventHandlers
{
    public class PayoutFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PayoutFailedEvent>>
    {
        public PayoutFailedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<PayoutFailedEvent> notification, CancellationToken token)
        {
            var payoutEvent = notification.DomainEvent;
            var payout = await _context.GetByIdAsync<Domain.Payout>(payoutEvent.PayoutId, token);
            if (payout.Status != TransactionStatus.Failed)
                return;

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Le paiement au producteur {payout.Author.Name} de {payout.Debited}€ a échoué",
               $"Le paiement au producteur {payout.Author.Name} ({payout.Author.Email}) de {payout.Debited}€ a échoué. Raison: {payout.ResultCode}-{payout.ResultMessage}.",
               false,
               token);
        }
    }
}