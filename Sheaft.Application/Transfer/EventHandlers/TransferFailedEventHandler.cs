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
            var transfer = await _context.GetByIdAsync<Transfer>(transferEvent.TransferId, token);
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