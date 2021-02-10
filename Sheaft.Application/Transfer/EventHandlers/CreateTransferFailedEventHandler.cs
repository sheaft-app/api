using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands.Handlers;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Handlers
{
    public class CreateTransferFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<CreateTransferFailedEvent>>
    {
        public CreateTransferFailedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<CreateTransferFailedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _emailService.SendEmailAsync(
                "support@sheaft.com",
                "Support",
                $"La création du transfert pour la commande {purchaseOrder.Reference} a échoué",
                $"La création du transfert de {purchaseOrder.TotalOnSalePrice}€ pour la commande {purchaseOrder.Reference} de {purchaseOrder.Vendor.Name} a échoué.",
                false,
                token);
        }
    }
}