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
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(ProductImportSucceededEvent productEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(productEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(ProductImportSucceededEvent), new { JobId = job.Id, UserId = job.User.Id });

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/products/";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre catalogue produit a bien été importé",
                nameof(ProductImportSucceededEvent),
                new ProductImportMailerModel { UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, PortalUrl = url },
                true,
                token);
        }

        public async Task Handle(ProductImportFailedEvent productEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(productEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(ProductImportFailedEvent), new { JobId = job.Id, UserId = job.User.Id });

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/jobs/{job.Id}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"L'import de votre catalogue produit a échoué",
                nameof(ProductImportSucceededEvent),
                new ProductImportMailerModel { UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, JobUrl = url },
                true,
                token);
        }

        public async Task Handle(ProductImportProcessingEvent productEvent, CancellationToken token)
        {
            var job = await _context.GetByIdAsync<Job>(productEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(ProductImportProcessingEvent), new { JobId = job.Id, UserId = job.User.Id });
        }
    }
}