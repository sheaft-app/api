using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{
    public class PickingOrderEventsHandler :
        INotificationHandler<PickingOrderExportSucceededEvent>,
        INotificationHandler<PickingOrderExportFailedEvent>,
        INotificationHandler<PickingOrderExportProcessingEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ISignalrService _signalrService;
        private readonly IConfiguration _configuration;

        public PickingOrderEventsHandler(IConfiguration configuration, IAppDbContext context, IEmailService emailService, ISignalrService signalrService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _signalrService = signalrService;
        }

        public async Task Handle(PickingOrderExportSucceededEvent pickingOrderEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(pickingOrderEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Company.Id, nameof(PickingOrderExportSucceededEvent), new { JobId = job.Id, Name = job.Name, UserId = job.User.Id, Url = job.File });

            var userName = $"{job.User.FirstName} {job.User.LastName}";
            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/jobs/{job.Id}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                userName,
                PickingOrderExportSucceededEvent.MAILING_TEMPLATE_ID,
                new { UserName = userName, Name = job.Name, job.CreatedOn, JobUrl = url, DownloadUrl = job.File },
                token);            
        }

        public async Task Handle(PickingOrderExportFailedEvent pickingOrderEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(pickingOrderEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Company.Id, nameof(PickingOrderExportFailedEvent), new { JobId = job.Id, Name = job.Name, UserId = job.User.Id });

            var userName = $"{job.User.FirstName} {job.User.LastName}";
            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/jobs/{job.Id}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                userName,
                PickingOrderExportFailedEvent.MAILING_TEMPLATE_ID,
                new { UserName = userName, Name = job.Name, job.CreatedOn, JobUrl = url },
                token);            
        }

        public async Task Handle(PickingOrderExportProcessingEvent pickingOrderEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(pickingOrderEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Company.Id, nameof(PickingOrderExportProcessingEvent), new { JobId = job.Id, Name = job.Name, UserId = job.User.Id });
        }
    }
}