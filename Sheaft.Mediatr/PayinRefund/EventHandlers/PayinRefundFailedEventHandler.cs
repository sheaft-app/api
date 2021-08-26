using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.PayinRefund;

namespace Sheaft.Mediatr.PayinRefund.EventHandlers
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
            var payinRefund = await _context.Set<Domain.PayinRefund>().SingleAsync(e => e.Id == orderEvent.RefundId, token);
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