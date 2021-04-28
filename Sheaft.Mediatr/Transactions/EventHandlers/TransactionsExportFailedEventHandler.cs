using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Mailings;
using Sheaft.Domain.Events.Transactions;

namespace Sheaft.Mediatr.Transactions.EventHandlers
{
    public class TransactionsExportFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<TransactionsExportFailedEvent>>
    {
        private readonly IConfiguration _configuration;

        public TransactionsExportFailedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<TransactionsExportFailedEvent> notification, CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(TransactionsExportFailedEvent), new { JobId = job.Id, Name = job.Name, UserId = job.UserId });

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/jobs/{job.Id:N}";
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