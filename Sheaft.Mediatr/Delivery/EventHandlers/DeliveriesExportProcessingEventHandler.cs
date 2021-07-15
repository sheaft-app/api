using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Domain.Events.PurchaseOrder;

namespace Sheaft.Mediatr.Delivery.EventHandlers
{
    public class DeliveriesExportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeliveriesExportProcessingEvent>>
    {
        public DeliveriesExportProcessingEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<DeliveriesExportProcessingEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.UserId, nameof(DeliveriesExportProcessingEvent),
                new {JobId = job.Id, Name = job.Name, UserId = job.UserId});
        }
    }
}