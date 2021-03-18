using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Product;

namespace Sheaft.Services.Product.EventHandlers
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