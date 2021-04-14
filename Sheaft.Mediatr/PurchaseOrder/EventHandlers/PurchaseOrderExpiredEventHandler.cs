using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.PurchaseOrder;

namespace Sheaft.Mediatr.PurchaseOrder.EventHandlers
{
    public class PurchaseOrderExpiredEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderExpiredEvent>>
    {
        private readonly IConfiguration _configuration;

        public PurchaseOrderExpiredEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrderExpiredEvent> notification,
            CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await NotifyProducerAsync(purchaseOrder, token);
            await NotifyConsumerAsync(purchaseOrder, token);
        }

        private async Task NotifyConsumerAsync(Domain.PurchaseOrder purchaseOrder, CancellationToken token)
        {
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id,
                nameof(PurchaseOrderExpiredEvent),
                purchaseOrder.GetPurchaseNotifModelAsString());

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Sender.Email,
                purchaseOrder.Sender.Name,
                $"Votre commande a expirée",
                nameof(PurchaseOrderExpiredEvent),
                purchaseOrder.GetTemplateData(
                    $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}"),
                true,
                token);
        }

        private async Task NotifyProducerAsync(Domain.PurchaseOrder purchaseOrder, CancellationToken token)
        {
            await _signalrService.SendNotificationToGroupAsync(purchaseOrder.Vendor.Id, "PurchaseOrderWithdrawnEvent",
                purchaseOrder.GetPurchaseNotifModelAsString());

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/purchase-orders/{purchaseOrder.Id}";
            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Vendor.Email,
                purchaseOrder.Vendor.Name,
                $"Votre commande a expirée",
                nameof(PurchaseOrderExpiredEvent),
                purchaseOrder.GetTemplateData(url),
                true,
                token);
        }
    }
}