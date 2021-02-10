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
    public class ProductImportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ProductImportSucceededEvent>>
    {
        private readonly IConfiguration _configuration;

        public ProductImportSucceededEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<ProductImportSucceededEvent> notification, CancellationToken token)
        {
            var productEvent = notification.DomainEvent;
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
    }
}