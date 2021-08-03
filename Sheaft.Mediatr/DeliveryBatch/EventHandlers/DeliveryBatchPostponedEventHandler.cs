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
    public class DeliveryBatchPostponedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<DeliveryBatchPostponedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public DeliveryBatchPostponedEventHandler(
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

        public async Task Handle(DomainEventNotification<DeliveryBatchPostponedEvent> notification, CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var deliveryBatch = await _context.DeliveryBatches.SingleAsync(d => d.Id == @event.DeliveryBatchId, token);
            if (deliveryBatch.Status != DeliveryBatchStatus.Ready && deliveryBatch.Status != DeliveryBatchStatus.Waiting)
                return;

            var deliveries = deliveryBatch.Deliveries
                .Where(d => d.Status != DeliveryStatus.Delivered && d.Status != DeliveryStatus.Rejected)
                .ToList();

            var clientIds = deliveries.Select(d => d.ClientId);
            var clients = await _context.Users
                .Where(u => clientIds.Contains(u.Id))
                .ToListAsync(token);
            
            foreach (var delivery in deliveries)
            {
                var client = clients.Single(c => c.Id == delivery.ClientId);
                await _signalrService.SendNotificationToUserAsync(client.Id, nameof(DeliveryPostponedEvent),
                    new {
                        Firstname = client.FirstName,
                        ScheduledOn = deliveryBatch.ScheduledOn,
                        ProducerName = deliveryBatch.AssignedTo.Name,
                        Url =$"{_configuration.GetValue<string>("Portal:url")}/#/expected-deliveries/{_idSerializer.Serialize("Query", nameof(Delivery), delivery.Id)}?refresh={Guid.NewGuid():N}" 
                    });
                
                await _emailService.SendTemplatedEmailAsync(
                    client.Email,
                    client.Name,
                    $"{deliveryBatch.AssignedTo.Name} a décalé sa livraison au {deliveryBatch.ScheduledOn.ToString("dd/MM/yyyy")}",
                    nameof(DeliveryPostponedEvent),
                    new DeliveryMailerModel
                    {
                        Firstname = client.FirstName,
                        ScheduledOn = deliveryBatch.ScheduledOn,
                        ProducerName = deliveryBatch.AssignedTo.Name,
                        Url =$"{_configuration.GetValue<string>("Portal:url")}/#/expected-deliveries/{_idSerializer.Serialize("Query", nameof(Delivery), delivery.Id)}?refresh={Guid.NewGuid():N}" 
                    },
                    true,
                    token);
            }
        }
    }
}