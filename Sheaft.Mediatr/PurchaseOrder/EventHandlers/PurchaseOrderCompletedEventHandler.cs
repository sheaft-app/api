using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.PurchaseOrder;

namespace Sheaft.Mediatr.PurchaseOrder.EventHandlers
{
    public class PurchaseOrderCompletedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderCompletedEvent>>
    {
        private readonly IConfiguration _configuration;

        public PurchaseOrderCompletedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrderCompletedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.SenderId, nameof(PurchaseOrderCompletedEvent), purchaseOrder.GetPurchaseNotifModelAsString());

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Sender.Email,
                purchaseOrder.Sender.Name,
                $"Votre commande pour {purchaseOrder.Vendor.Name} est prête !",
                nameof(PurchaseOrderCompletedEvent),
                purchaseOrder.GetTemplateData($"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}"),
                true,
                token);
        }
    }
}