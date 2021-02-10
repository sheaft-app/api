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
    public class PickingOrderExportFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<PickingOrderExportFailedEvent>>
    {
        private readonly IConfiguration _configuration;

        public PickingOrderExportFailedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<PickingOrderExportFailedEvent> notification, CancellationToken token)
        {
            var pickingOrderEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Job>(pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(PickingOrderExportFailedEvent), new { JobId = job.Id, Name = job.Name, UserId = job.User.Id });

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/jobs/{job.Id:N}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"La création de votre bon de préparation a échouée",
                nameof(PickingOrderExportFailedEvent),
                new PickingOrderExportMailerModel { UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, JobUrl = url },
                true,
                token);
        }
    }
}