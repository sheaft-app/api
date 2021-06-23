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
using Sheaft.Mailing;
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.Mediatr.Delivery.EventHandlers
{
    public class DeliveryFormGeneratedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeliveryFormGeneratedEvent>>
    {
        private readonly ISheaftMediatr _mediatr;

        public DeliveryFormGeneratedEventHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _mediatr = mediatr;
        }

        public async Task Handle(DomainEventNotification<DeliveryFormGeneratedEvent> notification,
            CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var delivery = await _context.Deliveries.SingleAsync(d => d.Id == @event.DeliveryId, token);
            if (delivery.Status != DeliveryStatus.Delivered)
                return;

            await _signalrService.SendNotificationToUserAsync(delivery.ClientId, nameof(DeliveryFormGeneratedEvent),
                new {Url = delivery.DeliveryFormUrl});

            var client = await _context.Users.SingleAsync(u => u.Id == delivery.ClientId, token);
            var producer = await _context.Producers.SingleAsync(u => u.Id == delivery.ProducerId, token);

            await _emailService.SendTemplatedEmailAsync(
                client.Email,
                client.Name,
                $"{producer.Name} - Bon de livraison du {delivery.DeliveredOn.Value:dd/MM/yyyy}",
                nameof(DeliveryFormGeneratedEvent),
                new DeliveryMailerModel
                {
                    Url = delivery.DeliveryFormUrl, DeliveredOn = delivery.DeliveredOn,
                    ProducerName = producer.Name, ScheduledOn = delivery.ScheduledOn
                },
                true,
                token);
        }
    }
}