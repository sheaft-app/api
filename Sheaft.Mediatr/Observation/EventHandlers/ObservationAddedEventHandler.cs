using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Observation;
using Sheaft.Mailing.Extensions;

namespace Sheaft.Mediatr.Observation.EventHandlers
{
    public class ObservationAddedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ObservationAddedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public ObservationAddedEventHandler(
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

        public async Task Handle(DomainEventNotification<ObservationAddedEvent> notification, CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var observation = await _context.Observations.SingleAsync(e => e.Id == @event.ObservationId, token);
            if (observation.User.Kind == ProfileKind.Producer)
                return;

            await _signalrService.SendNotificationToUserAsync(observation.ProducerId, nameof(ObservationAddedEvent),
                observation.GetNotificationContent(
                    _idSerializer.Serialize("Query", nameof(Observation), observation.Id),
                    _configuration.GetValue<string>("Portal:url"),
                    _idSerializer.Serialize("Query", nameof(Producer), observation.ProducerId)));

            await _emailService.SendTemplatedEmailAsync(
                observation.Producer.Email,
                observation.Producer.Name,
                $"{observation.User.Name} a ajouté une observation concernant un de vos produits ou lot.",
                nameof(ObservationAddedEvent),
                observation.GetNotificationData(
                    _idSerializer.Serialize("Query", nameof(Observation), observation.Id),
                    _configuration.GetValue<string>("Portal:url"),
                    observation.Comment,
                    _idSerializer.Serialize("Query", nameof(Producer), observation.ProducerId)),
                true,
                token);
        }
    }
}