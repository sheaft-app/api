using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.PurchaseOrder;

namespace Sheaft.Mediatr.PurchaseOrder.EventHandlers
{
    public class PurchaseOrdersExportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrdersExportProcessingEvent>>
    {
        private readonly IIdSerializer _idSerializer;

        public PurchaseOrdersExportProcessingEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            IIdSerializer idSerializer,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<PurchaseOrdersExportProcessingEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(PurchaseOrdersExportProcessingEvent),
                new {JobId = jobIdentifier, Name = job.Name, UserId = job.UserId});
        }
    }
}