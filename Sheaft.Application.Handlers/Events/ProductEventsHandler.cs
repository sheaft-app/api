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
    public class ProductEventsHandler : EventsHandler,
        INotificationHandler<ProductImportSucceededEvent>,
        INotificationHandler<ProductImportFailedEvent>,
        INotificationHandler<ProductImportProcessingEvent>
    {
        private readonly IConfiguration _configuration;

        public ProductEventsHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
        {
            _configuration = configuration;
        }

        public async Task Handle(ProductImportSucceededEvent productEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(productEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(ProductImportSucceededEvent), new { JobId = job.Id, UserId = job.User.Id });

            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/jobs/{job.Id}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                _emailTemplateOptions.ProductImportSucceededEvent,
                new { UserName = job.User.Name, Name = job.Name, job.CreatedOn, JobUrl = url, DownloadUrl = job.File },
                token);
        }

        public async Task Handle(ProductImportFailedEvent productEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(productEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(ProductImportFailedEvent), new { JobId = job.Id, UserId = job.User.Id });

            var url = $"{_configuration.GetValue<string>("Urls:Portal")}/#/jobs/{job.Id}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                _emailTemplateOptions.ProductImportFailedEvent,
                new { UserName = job.User.Name, Name = job.Name, job.CreatedOn, JobUrl = url },
                token);
        }

        public async Task Handle(ProductImportProcessingEvent productEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(productEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(ProductImportProcessingEvent), new { JobId = job.Id, UserId = job.User.Id });
        }
    }
}