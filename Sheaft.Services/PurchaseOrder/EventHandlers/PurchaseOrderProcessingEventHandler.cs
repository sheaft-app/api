using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.PurchaseOrder;

namespace Sheaft.Services.PurchaseOrder.EventHandlers
{
    public class PurchaseOrderProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrderProcessingEvent>>
    {
        public PurchaseOrderProcessingEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<PurchaseOrderProcessingEvent> notification,
            CancellationToken token)
        {
            var orderEvent = notification.DomainEvent;
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(orderEvent.PurchaseOrderId, token);
            await _signalrService.SendNotificationToUserAsync(purchaseOrder.Sender.Id,
                nameof(PurchaseOrderProcessingEvent), purchaseOrder.GetPurchaseNotifModelAsString());
        }
    }
}