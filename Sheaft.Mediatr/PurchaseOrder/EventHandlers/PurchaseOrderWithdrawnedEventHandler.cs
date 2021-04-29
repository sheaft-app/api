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
    public class PurchaseOrderWithdrawnedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderWithdrawnedEvent>>
    {
        private readonly IConfiguration _configuration;

        public PurchaseOrderWithdrawnedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrderWithdrawnedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == orderEvent.PurchaseOrderId, token);
            
            await _signalrService.SendNotificationToGroupAsync(purchaseOrder.ProducerId, nameof(PurchaseOrderWithdrawnedEvent), purchaseOrder.GetPurchaseNotifModelAsString());

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.SenderInfo.Email,
                purchaseOrder.SenderInfo.Name,
                $"Votre commande pour {purchaseOrder.VendorInfo.Name} prévue pour le {purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy} a bien été annulée",
                nameof(PurchaseOrderWithdrawnedEvent),
                purchaseOrder.GetTemplateData($"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}"),
                true,
                token);
            
            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.VendorInfo.Email,
                purchaseOrder.VendorInfo.Name,
                $"{purchaseOrder.SenderInfo.Name} a annulé sa commande pour le {purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy}",
                nameof(PurchaseOrderWithdrawnedEvent),
                purchaseOrder.GetTemplateData($"{_configuration.GetValue<string>("Portal:url")}/#/purchase-orders/{purchaseOrder.Id}"),
                true,
                token);
        }
    }
}