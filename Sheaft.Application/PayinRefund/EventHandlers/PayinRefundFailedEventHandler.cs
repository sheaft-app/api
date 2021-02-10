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
    public class PayinRefundFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PayinRefundFailedEvent>>
    {
        public PayinRefundFailedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<PayinRefundFailedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var payinRefund = await _context.GetByIdAsync<PayinRefund>(orderEvent.RefundId, token);
            if (payinRefund.Status != TransactionStatus.Failed)
                return;

            await _emailService.SendEmailAsync(
                "support@sheaft.com",
                "Support",
                $"Le paiement du remboursement de {payinRefund.Debited}€ {payinRefund.Author.Name} a échoué",
                $"Le paiement du remboursement de {payinRefund.Debited}€ {payinRefund.Author.Name} ({payinRefund.Author.Email}) a échoué. Raison: {payinRefund.ResultCode}-{payinRefund.ResultMessage}.",
                false,
                token);
        }
    }
}