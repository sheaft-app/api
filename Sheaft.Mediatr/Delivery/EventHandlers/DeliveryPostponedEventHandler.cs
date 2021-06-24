using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Declaration;
using Sheaft.Domain.Events.Delivery;

namespace Sheaft.Mediatr.Delivery.EventHandlers
{
    public class DeliveryPostponedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeliveryPostponedEvent>>
    {
        public DeliveryPostponedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }

        public Task Handle(DomainEventNotification<DeliveryPostponedEvent> notification, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}