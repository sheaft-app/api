using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Transactions;

namespace Sheaft.Mediatr.Transaction.EventHandlers
{
    public class TransactionsExportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<TransactionsExportProcessingEvent>>
    {
        public TransactionsExportProcessingEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<TransactionsExportProcessingEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.UserId, nameof(TransactionsExportProcessingEvent),
                new {JobId = job.Id, Name = job.Name, UserId = job.UserId});
        }
    }
}