using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Mailings;
using Sheaft.Domain.Events.Transactions;

namespace Sheaft.Mediatr.Transactions.EventHandlers
{
    public class TransactionsExportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<TransactionsExportSucceededEvent>>
    {
        public TransactionsExportSucceededEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<TransactionsExportSucceededEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.UserId, nameof(TransactionsExportSucceededEvent),
                new {JobId = job.Id, Name = job.Name, UserId = job.UserId, Url = job.File});

            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre export de virements est prêt",
                nameof(TransactionsExportSucceededEvent),
                new TransactionsExportMailerModel
                    {UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, DownloadUrl = job.File},
                true,
                token);
        }
    }
}