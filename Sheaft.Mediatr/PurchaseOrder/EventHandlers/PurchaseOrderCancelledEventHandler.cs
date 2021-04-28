using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.PurchaseOrder;

namespace Sheaft.Mediatr.PurchaseOrder.EventHandlers
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
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == orderEvent.PurchaseOrderId, token);
            
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.SenderId, nameof(PurchaseOrderCancelledEvent),
                purchaseOrder.GetPurchaseNotifModelAsString());

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Sender.Email,
                purchaseOrder.Sender.Name,
                $"{purchaseOrder.Vendor.Name} a annulé votre commande pour le {purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy}",
                nameof(PurchaseOrderCancelledEvent),
                purchaseOrder.GetTemplateData(
                    $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}"),
                true,
                token);
            
            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Vendor.Email,
                purchaseOrder.Vendor.Name,
                $"La commande de {purchaseOrder.Sender.Name} pour le {purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy} a bien été annulée",
                nameof(PurchaseOrderCancelledEvent),
                purchaseOrder.GetTemplateData(
                    $"{_configuration.GetValue<string>("Portal:url")}/#/purchase-orders/{purchaseOrder.Id}"),
                true,
                token);
        }
    }
}