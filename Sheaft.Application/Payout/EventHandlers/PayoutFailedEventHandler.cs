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
            var payout = await _context.GetByIdAsync<Payout>(payoutEvent.PayoutId, token);
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