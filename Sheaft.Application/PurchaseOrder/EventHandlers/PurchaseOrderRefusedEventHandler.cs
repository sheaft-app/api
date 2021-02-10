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
    public class PurchaseOrderRefusedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderRefusedEvent>>
    {
        private readonly IConfiguration _configuration;

        public PurchaseOrderRefusedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrderRefusedEvent> notification, CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id, nameof(PurchaseOrderRefusedEvent), purchaseOrder.GetPurchaseNotifModelAsString());

            await _emailService.SendTemplatedEmailAsync(
                purchaseOrder.Sender.Email,
                purchaseOrder.Sender.Name,
                $"Votre commande a été annulée par {purchaseOrder.Vendor.Name}",
                nameof(PurchaseOrderRefusedEvent),
                purchaseOrder.GetTemplateDatas($"{_configuration.GetValue<string>("Portal:url")}/#/my-orders/{purchaseOrder.Id}"),
                true,
                token);
        }
    }
}