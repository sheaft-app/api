using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Events.PickingOrder;
using Sheaft.Domain.Events.Transactions;

namespace Sheaft.Application.Transactions.EventHandlers
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
            var job = await _context.GetByIdAsync<Domain.Job>(pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(TransactionsExportProcessingEvent),
                new {JobId = job.Id, Name = job.Name, UserId = job.User.Id});
        }
    }
}