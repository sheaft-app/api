using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Events.PurchaseOrder;

namespace Sheaft.Application.PurchaseOrder.EventHandlers
{
    public class PurchaseOrderCancelledEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderCancelledEvent>>
    {
        private readonly IConfiguration _configuration;

        public PurchaseOrderCancelledEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrderCancelledEvent> notification, CancellationToken token)
        {
            await NotifyConsumerAsync(notification, token);
            await NotifyProducerAsync(notification, token);
        }

        private async Task NotifyConsumerAsync(DomainEventNotification<PurchaseOrderCancelledEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderCancelledEvent),
                purchaseOrder.GetPurchaseNotifModelAsString());

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Sender.Email,
                purchaseOrder.Sender.Name,
                $"{purchaseOrder.Vendor.Name} a annulé votre commande",
                nameof(PurchaseOrderCancelledEvent),
                purchaseOrder.GetTemplateDatas(
                    $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}"),
                true,
                token);
        }

        private async Task NotifyProducerAsync(DomainEventNotification<PurchaseOrderCancelledEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToGroupAsync(purchaseOrder.Vendor.Id, "PurchaseOrderWithdrawnEvent", purchaseOrder.GetPurchaseNotifModelAsString());

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/purchase-orders/{purchaseOrder.Id}";
            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Vendor.Email,
                purchaseOrder.Vendor.Name,
                $"{purchaseOrder.Sender.Name} a annulé sa commande pour le {purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy}",
                "PurchaseOrderWithdrawnEvent",
                purchaseOrder.GetTemplateDatas(url),
                true,
                token);
        }
    }
}