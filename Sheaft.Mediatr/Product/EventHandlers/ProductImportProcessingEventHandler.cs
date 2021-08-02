using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Product;

namespace Sheaft.Mediatr.Product.EventHandlers
{
    public class ProductImportProcessingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ProductImportProcessingEvent>>
    {
        private readonly IIdSerializer _idSerializer;

        public ProductImportProcessingEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            IIdSerializer idSerializer,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<ProductImportProcessingEvent> notification,
            CancellationToken token)
        {
            var productEvent = notification.DomainEvent;
            var job = await _context.Jobs.SingleAsync(e => e.Id == productEvent.JobId, token);
            var jobIdentifier = _idSerializer.Serialize("Query", nameof(Job), job.Id);
            
            await _signalrService.SendNotificationToUserAsync(job.UserId, nameof(ProductImportProcessingEvent),
                new {JobId = jobIdentifier, UserId = job.UserId});
        }
    }
}