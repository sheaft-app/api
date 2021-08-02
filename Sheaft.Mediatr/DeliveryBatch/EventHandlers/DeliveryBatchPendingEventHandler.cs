using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Declaration;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Domain.Events.DeliveryBatch;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.DeliveryBatch.EventHandlers
{
    public class DeliveryBatchPendingEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeliveryBatchPendingEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public DeliveryBatchPendingEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IIdSerializer idSerializer,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<DeliveryBatchPendingEvent> notification,
            CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var deliveryBatch = await _context.DeliveryBatches.SingleAsync(d => d.Id == @event.DeliveryBatchId, token);
            if (deliveryBatch.Status is DeliveryBatchStatus.Completed or DeliveryBatchStatus.Cancelled or
                DeliveryBatchStatus.Partial)
                return;

            var deliveryBatchIdentifier =
                _idSerializer.Serialize("Query", nameof(Domain.DeliveryBatch), deliveryBatch.Id);
            
            await _signalrService.SendNotificationToUserAsync(deliveryBatch.AssignedToId,
                nameof(DeliveryPostponedEvent),
                new
                {
                    Id = deliveryBatchIdentifier,
                    Firstname = deliveryBatch.AssignedTo.FirstName,
                    ScheduledOn = deliveryBatch.ScheduledOn,
                    ProducerName = deliveryBatch.AssignedTo.Name,
                    Name = deliveryBatch.Name,
                    Url =
                        $"{_configuration.GetValue<string>("Portal:url")}/#/delivery-batches/{deliveryBatchIdentifier}?refresh={Guid.NewGuid():N}"
                });

            await _emailService.SendTemplatedEmailAsync(
                deliveryBatch.AssignedTo.Email,
                deliveryBatch.AssignedTo.Name,
                $"Votre livraison {deliveryBatch.Name} du {deliveryBatch.ScheduledOn.ToString("dd/MM/yyyy")} est toujours en attente",
                nameof(DeliveryBatchPendingEvent),
                new DeliveryBatchMailerModel
                {
                    Id = deliveryBatchIdentifier,
                    Firstname = deliveryBatch.AssignedTo.FirstName,
                    ScheduledOn = deliveryBatch.ScheduledOn,
                    ProducerName = deliveryBatch.AssignedTo.Name,
                    Name = deliveryBatch.Name,
                    Url =
                        $"{_configuration.GetValue<string>("Portal:url")}/#/delivery-batches/{deliveryBatchIdentifier}?refresh={Guid.NewGuid():N}"
                },
                true,
                token);
        }
    }
}