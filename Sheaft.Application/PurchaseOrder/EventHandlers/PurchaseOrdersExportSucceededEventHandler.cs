using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models.Mailer;
using Sheaft.Domain.Events.Transactions;

namespace Sheaft.Application.Transactions.EventHandlers
{
    public class PurchaseOrdersExportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PurchaseOrdersExportSucceededEvent>>
    {
        public PurchaseOrdersExportSucceededEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<PurchaseOrdersExportSucceededEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Domain.Job>(pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(PurchaseOrdersExportSucceededEvent),
                new {JobId = job.Id, Name = job.Name, UserId = job.User.Id, Url = job.File});

            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre export de commandes est prêt",
                nameof(PurchaseOrdersExportSucceededEvent),
                new PurchaseOrdersExportMailerModel
                    {UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, DownloadUrl = job.File},
                true,
                token);
        }
    }
}