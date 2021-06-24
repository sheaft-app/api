using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Declaration;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.Mediatr.Delivery.EventHandlers
{
    public class DeliveryReadyEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeliveryReadyEvent>>
    {
        private readonly ISheaftMediatr _mediatr;

        public DeliveryReadyEventHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _mediatr = mediatr;
        }

        public Task Handle(DomainEventNotification<DeliveryReadyEvent> notification, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}