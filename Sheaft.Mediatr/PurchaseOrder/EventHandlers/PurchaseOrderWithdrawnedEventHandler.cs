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
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            
            await _signalrService.SendNotificationToGroupAsync(purchaseOrder.Vendor.Id, "PurchaseOrderWithdrawnEvent", purchaseOrder.GetPurchaseNotifModelAsString());

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/purchase-orders/{purchaseOrder.Id}";
            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Vendor.Email,
                purchaseOrder.Vendor.Name,
                $"{purchaseOrder.Sender.Name} a annulé sa commande pour le {purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy}",
                nameof(PurchaseOrderWithdrawnedEvent),
                purchaseOrder.GetTemplateData(url),
                true,
                token);
        }
    }
}