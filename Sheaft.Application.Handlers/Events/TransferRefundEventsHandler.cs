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
    public class TransferRefundEventsHandler : EventsHandler,
        INotificationHandler<TransferRefundFailedEvent>,
        INotificationHandler<CreateTransferRefundFailedEvent>
    {
        public TransferRefundEventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(TransferRefundFailedEvent notification, CancellationToken token)
        {
            var payinRefund = await _context.GetByIdAsync<TransferRefund>(notification.RefundId, token);
            if (payinRefund.Status != TransactionStatus.Failed)
                return;

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"Le paiement du remboursement du transfert de {payinRefund.Debited}€ {payinRefund.Author.Name} a échoué",
               $"Le paiement du remboursement du transfert de {payinRefund.Debited}€ {payinRefund.Author.Name} ({payinRefund.Author.Email}) a échoué. Raison: {payinRefund.ResultCode}-{payinRefund.ResultMessage}.",
               false,
               token);
        }

        public async Task Handle(CreateTransferRefundFailedEvent notification, CancellationToken token)
        {
            var order = await _context.GetByIdAsync<PurchaseOrder>(notification.PurchaseOrderId, token);

            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La création du remboursement du transfert de la purchaseorder {order.Reference} pour {order.Vendor.Name} a échouée",
               $"La création du remboursement du transfert de la purchaseorder {order.Reference} pour {order.Vendor.Name}({order.Vendor.Email}) a échouée à plus de 3 reprises, le remboursement du paiement est mis en supsend, veuillez traiter manuellement la demande.",
               false,
               token);
        }
    }
}