using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Domain.Events.Transactions;

namespace Sheaft.Application.Transactions.EventHandlers
{
    public class PurchaseOrdersExportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrdersExportProcessingEvent>>
    {
        public PurchaseOrdersExportProcessingEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<PurchaseOrdersExportProcessingEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Domain.Job>(pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(PurchaseOrdersExportProcessingEvent),
                new {JobId = job.Id, Name = job.Name, UserId = job.User.Id});
        }
    }
}