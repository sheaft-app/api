using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Mailing.Extensions;

namespace Sheaft.Mediatr.PurchaseOrder.EventHandlers
{
    public class PurchaseOrderWithdrawnedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderWithdrawnedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public PurchaseOrderWithdrawnedEventHandler(
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

        public async Task Handle(DomainEventNotification<PurchaseOrderWithdrawnedEvent> notification,
            CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder =
                await _context.PurchaseOrders.SingleAsync(e => e.Id == orderEvent.PurchaseOrderId, token);

            var purchaseOrderIdentifier = _idSerializer.Serialize("Query", nameof(PurchaseOrder), purchaseOrder.Id);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.ProducerId,
                nameof(PurchaseOrderWithdrawnedEvent), purchaseOrder.GetPurchaseNotifModelAsString(purchaseOrderIdentifier));

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.SenderInfo.Email,
                purchaseOrder.SenderInfo.Name,
                $"Votre commande pour {purchaseOrder.VendorInfo.Name} prévue pour le {purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy} a bien été annulée",
                nameof(PurchaseOrderWithdrawnedEvent),
                purchaseOrder.GetTemplateData(purchaseOrderIdentifier,
                    $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrderIdentifier}?refresh={Guid.NewGuid():N}"),
                true,
                token);

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.VendorInfo.Email,
                purchaseOrder.VendorInfo.Name,
                $"{purchaseOrder.SenderInfo.Name} a annulé sa commande pour le {purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy}",
                nameof(PurchaseOrderWithdrawnedEvent),
                purchaseOrder.GetTemplateData(purchaseOrderIdentifier, 
                    $"{_configuration.GetValue<string>("Portal:url")}/#/purchase-orders/{purchaseOrderIdentifier}?refresh={Guid.NewGuid():N}"),
                true,
                token);
        }
    }
}