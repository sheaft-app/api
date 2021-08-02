using System;
using System.Threading;
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
    public class PurchaseOrderAcceptedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderAcceptedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public PurchaseOrderAcceptedEventHandler(
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

        public async Task Handle(DomainEventNotification<PurchaseOrderAcceptedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == orderEvent.PurchaseOrderId, token);
            
            var purchaseOrderIdentifier = _idSerializer.Serialize("Query", nameof(PurchaseOrder), purchaseOrder.Id);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.ClientId, nameof(PurchaseOrderAcceptedEvent), purchaseOrder.GetPurchaseNotifModelAsString(purchaseOrderIdentifier));

            var email = purchaseOrder.SenderInfo.Email;
            var name = purchaseOrder.SenderInfo.Name;
            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrderIdentifier}?refresh={Guid.NewGuid():N}";

            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                $"{purchaseOrder.VendorInfo.Name} a accepté votre commande",
                nameof(PurchaseOrderAcceptedEvent),
                purchaseOrder.GetTemplateData(purchaseOrderIdentifier, url),
                true,
                token);
        }
    }
}