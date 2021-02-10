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
    public class PurchaseOrderAcceptedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderAcceptedEvent>>
    {
        private readonly IConfiguration _configuration;

        public PurchaseOrderAcceptedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrderAcceptedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderAcceptedEvent), purchaseOrder.GetPurchaseNotifModelAsString());

            var email = purchaseOrder.Sender.Email;
            var name = purchaseOrder.Sender.Name;
            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}";

            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                $"{purchaseOrder.Vendor.Name} a accepté votre commande",
                nameof(PurchaseOrderAcceptedEvent),
                purchaseOrder.GetTemplateDatas(url),
                true,
                token);
        }
    }
}