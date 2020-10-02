using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Microsoft.Extensions.Options;
using Sheaft.Options;

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
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
            _configuration = configuration;
        }

        public async Task Handle(PickingOrderExportSucceededEvent pickingOrderEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(PickingOrderExportSucceededEvent), new { JobId = job.Id, Name = job.Name, UserId = job.User.Id, Url = job.File });

            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/jobs/{job.Id}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                _emailTemplateOptions.PickingOrderExportSucceededEvent,
                new { UserName = job.User.Name, Name = job.Name, job.CreatedOn, JobUrl = url, DownloadUrl = job.File },
                token);            
        }

        public async Task Handle(PickingOrderExportFailedEvent pickingOrderEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(PickingOrderExportFailedEvent), new { JobId = job.Id, Name = job.Name, UserId = job.User.Id });

            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/jobs/{job.Id}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                _emailTemplateOptions.PickingOrderExportFailedEvent,
                new { UserName = job.User.Name, Name = job.Name, job.CreatedOn, JobUrl = url },
                token);            
        }

        public async Task Handle(PickingOrderExportProcessingEvent pickingOrderEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(pickingOrderEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(PickingOrderExportProcessingEvent), new { JobId = job.Id, Name = job.Name, UserId = job.User.Id });
        }
    }
}