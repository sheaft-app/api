﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Mailing.Extensions;

namespace Sheaft.Mediatr.PurchaseOrder.EventHandlers
{
    public class PurchaseOrderRefusedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderRefusedEvent>>
    {
        private readonly IConfiguration _configuration;

        public PurchaseOrderRefusedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrderRefusedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == orderEvent.PurchaseOrderId, token);
            
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.ClientId, nameof(PurchaseOrderRefusedEvent), purchaseOrder.GetPurchaseNotifModelAsString());

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.SenderInfo.Email,
                purchaseOrder.SenderInfo.Name,
                $"Votre commande a été annulée par {purchaseOrder.VendorInfo.Name}",
                nameof(PurchaseOrderRefusedEvent),
                purchaseOrder.GetTemplateData($"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}"),
                true,
                token);
        }
    }
}