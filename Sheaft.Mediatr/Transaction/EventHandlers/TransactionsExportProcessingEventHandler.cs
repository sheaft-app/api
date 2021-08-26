using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Events.Transactions;

namespace Sheaft.Mediatr.Transaction.EventHandlers
{
    public class TransactionsExportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<TransactionsExportProcessingEvent>>
    {
        private readonly IIdSerializer _idSerializer;

        public TransactionsExportProcessingEventHandler(
            IAppDbContext context,
            IIdSerializer idSerializer,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<TransactionsExportProcessingEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(TransactionsExportProcessingEvent),
                new {JobId = jobIdentifier, Name = job.Name, UserId = job.UserId});
        }
    }
}