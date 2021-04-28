using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Product;

namespace Sheaft.Mediatr.Product.EventHandlers
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
            var job = await _context.Jobs.SingleAsync(e => e.Id == productEvent.JobId, token);
            await _signalrService.SendNotificationToGroupAsync(job.UserId, nameof(ProductImportProcessingEvent),
                new {JobId = job.Id, UserId = job.UserId});
        }
    }
}