using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Common.Handlers;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Mailer;
using Sheaft.Domain.Events.Product;

namespace Sheaft.Application.Product.EventHandlers
{
    public class ProductImportFailedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ProductImportFailedEvent>>
    {
        private readonly IConfiguration _configuration;

        public ProductImportFailedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<ProductImportFailedEvent> notification, CancellationToken token)
        {
            var productEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Domain.Job>(productEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(ProductImportFailedEvent), new { JobId = job.Id, UserId = job.User.Id });

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/jobs/{job.Id}";
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"L'import de votre catalogue produit a échoué",
                nameof(ProductImportFailedEvent),
                new ProductImportMailerModel { UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, JobUrl = url },
                true,
                token);
        }
    }
}