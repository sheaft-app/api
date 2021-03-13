using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Mailer;
using Sheaft.Domain.Events.PickingOrder;

namespace Sheaft.Application.PickingOrders.EventHandlers
{
    public class PickingOrderExportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PickingOrderExportSucceededEvent>>
    {
        public PickingOrderExportSucceededEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<PickingOrderExportSucceededEvent> notification,
            CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Domain.Job>(pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(PickingOrderExportSucceededEvent),
                new {JobId = job.Id, Name = job.Name, UserId = job.User.Id, Url = job.File});

            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre bon de préparation est prêt",
                nameof(PickingOrderExportSucceededEvent),
                new PickingOrderExportMailerModel
                    {UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, DownloadUrl = job.File},
                true,
                token);
        }
    }
}