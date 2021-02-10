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
    public class PurchaseOrderWithdrawnEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderWithdrawnEvent>>
    {
        private readonly IConfiguration _configuration;

        public PurchaseOrderWithdrawnEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrderWithdrawnEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToGroupAsync(purchaseOrder.Vendor.Id, nameof(PurchaseOrderWithdrawnEvent), purchaseOrder.GetPurchaseNotifModelAsString());

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/purchase-orders/{purchaseOrder.Id}";
            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Vendor.Email,
                purchaseOrder.Vendor.Name,
                $"{purchaseOrder.Sender.Name} a annulé sa commande pour le {purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate:dd/MM/yyyy}",
                nameof(PurchaseOrderWithdrawnEvent),
                purchaseOrder.GetTemplateDatas(url),
                true,
                token);
        }
    }
}