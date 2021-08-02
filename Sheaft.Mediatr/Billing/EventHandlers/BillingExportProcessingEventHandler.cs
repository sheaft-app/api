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
using Sheaft.Domain.Events.Delivery;
using Sheaft.Domain.Events.PurchaseOrder;

namespace Sheaft.Mediatr.Billing.EventHandlers
{
    public class BillingExportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<BillingExportProcessingEvent>>
    {
        private readonly IIdSerializer _idSerializer;

        public BillingExportProcessingEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            IIdSerializer idSerializer,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<BillingExportProcessingEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(BillingExportProcessingEvent),
                new {JobId = job.Id, Name = job.Name, UserId = job.UserId});
        }
    }
}