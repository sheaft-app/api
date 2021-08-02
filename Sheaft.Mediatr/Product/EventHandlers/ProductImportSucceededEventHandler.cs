using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Product;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.Product.EventHandlers
{
    public class ProductImportSucceededEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ProductImportSucceededEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public ProductImportSucceededEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            IIdSerializer idSerializer,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<ProductImportSucceededEvent> notification, CancellationToken token)
        {
            var productEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == productEvent.JobId, token);
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(ProductImportSucceededEvent), new { JobId = job.Id, UserId = job.UserId });

            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/jobs/{jobIdentifier}?refresh={Guid.NewGuid():N}";
            
            await _emailService.SendTemplatedEmailAsync(
                job.User.Email,
                job.User.Name,
                $"Votre catalogue produit a bien été importé",
                nameof(ProductImportSucceededEvent),
                new ProductImportMailerModel { JobId = jobIdentifier, UserName = job.User.Name, Name = job.Name, CreatedOn = job.CreatedOn, PortalUrl = url },
                true,
                token);
        }
    }
}