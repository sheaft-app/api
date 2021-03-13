﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Events.Transfer;

namespace Sheaft.Application.Transfer.EventHandlers
{
    public class CreateTransferFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<CreatePurchaseOrderTransferFailedEvent>>
    {
        public CreateTransferFailedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<CreatePurchaseOrderTransferFailedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(orderEvent.PurchaseOrderId, token);
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