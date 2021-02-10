using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Commands.Handlers;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Application.Models.Mailer;

namespace Sheaft.Application.Handlers
{
    public class PickingOrderExportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PickingOrderExportSucceededEvent>>
    {
        private readonly IConfiguration _configuration;

        public PickingOrderExportSucceededEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PickingOrderExportSucceededEvent> notification, CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Job>(pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(PickingOrderExportSucceededEvent), new { JobId = job.Id, Name = job.Name, UserId = job.User.Id, Url = job.File });

            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre bon de préparation est prêt",
                nameof(PickingOrderExportSucceededEvent),
                new PickingOrderExportMailerModel { UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, DownloadUrl = job.File },
                true,
                token);
        }
    }
}