using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sheaft.Application.Commands.Handlers;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Application.Models.Mailer;

namespace Sheaft.Application.Handlers
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
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
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