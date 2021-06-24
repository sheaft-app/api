using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.DeliveryBatch;

namespace Sheaft.Mediatr.DeliveryBatch.EventHandlers
{
    public class DeliveryBatchStartedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeliveryBatchStartedEvent>>
    {
        public DeliveryBatchStartedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public Task Handle(DomainEventNotification<DeliveryBatchStartedEvent> notification,
            CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}