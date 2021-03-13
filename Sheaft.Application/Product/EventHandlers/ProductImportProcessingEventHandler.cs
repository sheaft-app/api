using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Events.Product;

namespace Sheaft.Application.Product.EventHandlers
{
    public class ProductImportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ProductImportProcessingEvent>>
    {
        public ProductImportProcessingEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public async Task Handle(DomainEventNotification<ProductImportProcessingEvent> notification,
            CancellationToken token)
        {
            var productEvent = notification.DomainEvent;
            var job = await _context.GetByIdAsync<Domain.Job>(productEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.User.Id, nameof(ProductImportProcessingEvent),
                new {JobId = job.Id, UserId = job.User.Id});
        }
    }
}