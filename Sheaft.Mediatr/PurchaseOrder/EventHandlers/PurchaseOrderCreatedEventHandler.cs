﻿using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
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
    public class PurchaseOrderCreatedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderCreatedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public PurchaseOrderCreatedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IIdSerializer idSerializer,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrderCreatedEvent> notification,
            CancellationToken token)
        {
            await NotifyProducerAsync(notification, token);
        }

        private async Task NotifyProducerAsync(DomainEventNotification<PurchaseOrderCreatedEvent> notification,
            CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder =
                await _context.PurchaseOrders.SingleAsync(e => e.Id == orderEvent.PurchaseOrderId, token);

            var purchaseOrderIdentifier = _idSerializer.Serialize("Query", nameof(PurchaseOrder), purchaseOrder.Id);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.ProducerId, "PurchaseOrderReceivedEvent",
                purchaseOrder.GetPurchaseNotifModelAsString(purchaseOrderIdentifier));

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.VendorInfo.Email,
                purchaseOrder.VendorInfo.Name,
                $"{purchaseOrder.SenderInfo.Name} a envoyé une commande pour le {purchaseOrder.Delivery.ExpectedDeliveryDate:dd/MM/yyyy}",
                "PurchaseOrderReceivedEvent",
                purchaseOrder.GetTemplateData(purchaseOrderIdentifier,
                    $"{_configuration.GetValue<string>("Portal:url")}/#/purchase-orders/{purchaseOrderIdentifier}"),
                true,
                token);
        }
    }
}