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
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Mailing.Extensions;

namespace Sheaft.Mediatr.PurchaseOrder.EventHandlers
{
    public class PurchaseOrderExpiredEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderExpiredEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public PurchaseOrderExpiredEventHandler(
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

        public async Task Handle(DomainEventNotification<PurchaseOrderExpiredEvent> notification,
            CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder =
                await _context.PurchaseOrders.SingleAsync(e => e.Id == orderEvent.PurchaseOrderId, token);
            await NotifyProducerAsync(purchaseOrder, token);
            await NotifyConsumerAsync(purchaseOrder, token);
        }

        private async Task NotifyConsumerAsync(Domain.PurchaseOrder purchaseOrder, CancellationToken token)
        {
            var purchaseOrderIdentifier = _idSerializer.Serialize("Query", nameof(PurchaseOrder), purchaseOrder.Id);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.ClientId,
                nameof(PurchaseOrderExpiredEvent),
                purchaseOrder.GetPurchaseNotifModelAsString(purchaseOrderIdentifier));

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.SenderInfo.Email,
                purchaseOrder.SenderInfo.Name,
                $"Votre commande a expirée",
                nameof(PurchaseOrderExpiredEvent),
                purchaseOrder.GetTemplateData(purchaseOrderIdentifier,
                    $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrderIdentifier}"),
                true,
                token);
        }

        private async Task NotifyProducerAsync(Domain.PurchaseOrder purchaseOrder, CancellationToken token)
        {
            var purchaseOrderIdentifier = _idSerializer.Serialize("Query", nameof(PurchaseOrder), purchaseOrder.Id);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.ProducerId, "PurchaseOrderWithdrawnEvent",
                purchaseOrder.GetPurchaseNotifModelAsString(purchaseOrderIdentifier));

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/purchase-orders/{purchaseOrderIdentifier}";
            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.VendorInfo.Email,
                purchaseOrder.VendorInfo.Name,
                $"Votre commande a expirée",
                nameof(PurchaseOrderExpiredEvent),
                purchaseOrder.GetTemplateData(purchaseOrderIdentifier, url),
                true,
                token);
        }
    }
}