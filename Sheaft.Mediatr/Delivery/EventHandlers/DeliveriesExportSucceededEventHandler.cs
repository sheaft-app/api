using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.Delivery.EventHandlers
{
    public class DeliveriesExportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeliveriesExportSucceededEvent>>
    {
        public DeliveriesExportSucceededEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<DeliveriesExportSucceededEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == pickingOrderEvent.JobId, token);
            
            await _signalrService.SendNotificationToGroupAsync(job.UserId, nameof(DeliveriesExportSucceededEvent),
                new {JobId = job.Id, Name = job.Name, UserId = job.UserId, Url = job.File});

            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre export de livraisons est prêt",
                nameof(DeliveriesExportSucceededEvent),
                new DeliveriesExportMailerModel
                    {UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, DownloadUrl = job.File},
                true,
                token);
        }
    }
}