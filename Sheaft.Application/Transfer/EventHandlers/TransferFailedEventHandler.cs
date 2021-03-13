using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Transfer;

namespace Sheaft.Application.Transfer.EventHandlers
{
    public class TransferFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<TransferFailedEvent>>
    {
        public TransferFailedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<TransferFailedEvent> notification, CancellationToken token)
        {
            var transferEvent = notification.DomainEvent;
            var transfer = await _context.GetByIdAsync<Domain.Transfer>(transferEvent.TransferId, token);
            if (transfer.Status != TransactionStatus.Failed)
                return;

            await _emailService.SendEmailAsync(
                "support@sheaft.com",
                "Support",
                $"Le paiement de {transfer.Debited}€ de la commande {transfer.PurchaseOrder.Reference} au producteur {transfer.Author.Name} a échoué",
                $"Le paiement de {transfer.Debited}€ de la commande {transfer.PurchaseOrder.Reference} au producteur {transfer.Author.Name}({transfer.Author.Email}) a échoué. Raison: {transfer.ResultCode}-{transfer.ResultMessage}.",
                false,
                token);
        }
    }
}