using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Transactions;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.Transaction.EventHandlers
{
    public class TransactionsExportFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<TransactionsExportFailedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public TransactionsExportFailedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IIdSerializer idSerializer,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<TransactionsExportFailedEvent> notification, CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(TransactionsExportFailedEvent), new { JobId = jobIdentifier, Name = job.Name, UserId = job.UserId });

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/jobs/{jobIdentifier}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"La création de votre export de virements a échouée",
                nameof(TransactionsExportFailedEvent),
                new TransactionsExportMailerModel { UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, JobUrl = url },
                true,
                token);
        }
    }
}