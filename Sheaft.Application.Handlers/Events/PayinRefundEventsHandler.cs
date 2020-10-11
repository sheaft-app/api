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
    public class PayinRefundEventsHandler : EventsHandler,
        INotificationHandler<PayinRefundFailedEvent>,
        INotificationHandler<CreatePayinRefundFailedEvent>
    {
        public PayinRefundEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(PayinRefundFailedEvent notification, CancellationToken token)
        {
            var payinRefund = await _context.GetByIdAsync<PayinRefund>(notification.RefundId, token);
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

        public async Task Handle(CreatePayinRefundFailedEvent notification, CancellationToken token)
        {
            var order = await _context.GetByIdAsync<Order>(notification.OrderId, token);

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