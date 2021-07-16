using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Batch;
using Sheaft.Mailing.Extensions;

namespace Sheaft.Mediatr.Batch.EventHandlers
{
    public class BatchObservationAddedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<BatchObservationAddedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public BatchObservationAddedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            IIdSerializer idSerializer,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
            _idSerializer = idSerializer;
        }

        public async Task Handle(DomainEventNotification<BatchObservationAddedEvent> notification, CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var batch = await _context.Set<Domain.Batch>().SingleAsync(e => e.Id == @event.BatchId, token);
            var batchObservation = await _context.Set<Domain.BatchObservation>().SingleAsync(e => e.Id == @event.BatchObservationId, token);

            if (batchObservation.User.Kind == ProfileKind.Producer)
                return;
            
            await _signalrService.SendNotificationToGroupAsync(batch.ProducerId, nameof(BatchObservationAddedEvent), batch.GetObservationNotificationContent(
                _idSerializer.Serialize("Query", nameof(Batch), batch.Id), 
                _idSerializer.Serialize("Query", nameof(BatchObservation), batchObservation.Id),
                _configuration.GetValue<string>("Portal:url"),
                batchObservation.User.Name));
            
            await _emailService.SendTemplatedEmailAsync(
                batch.Producer.Email,
                batch.Producer.Name,
                $"{batchObservation.User.Name} a ajouté une remarque concernant le lot {batch.Number}",
                nameof(BatchObservationAddedEvent),
                batch.GetObservationNotificationData(
                    _idSerializer.Serialize("Query", nameof(Batch), batch.Id), 
                    _idSerializer.Serialize("Query", nameof(BatchObservation), batchObservation.Id),
                    _configuration.GetValue<string>("Portal:url"),
                    batchObservation.Comment,
                    batchObservation.User.Name),
                true,
                token);
        }
    }
}