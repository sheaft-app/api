using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.PickingOrder;

namespace Sheaft.Mediatr.PickingOrder.EventHandlers
{
    public class PickingOrderExportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PickingOrderExportProcessingEvent>>
    {
        private readonly IIdSerializer _idSerializer;

        public PickingOrderExportProcessingEventHandler(
            IAppDbContext context,
            IIdSerializer idSerializer,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<PickingOrderExportProcessingEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(PickingOrderExportProcessingEvent),
                new {JobId = jobIdentifier, Name = job.Name, UserId = job.UserId});
        }
    }
}