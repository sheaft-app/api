using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Sheaft.Application.Models.Mailer;

namespace Sheaft.Application.Handlers
{
    public class PickingOrderEventsHandler : EventsHandler,
        INotificationHandler<PickingOrderExportSucceededEvent>,
        INotificationHandler<PickingOrderExportFailedEvent>,
        INotificationHandler<PickingOrderExportProcessingEvent>
    {
        private readonly IConfiguration _configuration;

        public PickingOrderEventsHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(PickingOrderExportSucceededEvent pickingOrderEvent, CancellationToken token)
        {
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

        public async Task Handle(PickingOrderExportFailedEvent pickingOrderEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(PickingOrderExportFailedEvent), new { JobId = job.Id, Name = job.Name, UserId = job.User.Id });

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/purchase-orders";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"La génération de votre bon de préparation a échouée",
                nameof(PickingOrderExportFailedEvent),
                new PickingOrderExportMailerModel { UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, JobUrl = url },
                true,
                token);
        }

        public async Task Handle(PickingOrderExportProcessingEvent pickingOrderEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(PickingOrderExportProcessingEvent), new { JobId = job.Id, Name = job.Name, UserId = job.User.Id });
        }
    }
}