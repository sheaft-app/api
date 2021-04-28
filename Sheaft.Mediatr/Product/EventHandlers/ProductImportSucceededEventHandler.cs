using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Mailings;
using Sheaft.Domain.Events.Product;

namespace Sheaft.Mediatr.Product.EventHandlers
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
            var job = await _context.Jobs.SingleAsync(e => e.Id == productEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.UserId, nameof(ProductImportSucceededEvent), new { JobId = job.Id, UserId = job.UserId });

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