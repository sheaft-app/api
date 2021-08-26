using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Observation;
using Sheaft.Mailing.Extensions;

namespace Sheaft.Mediatr.Observation.EventHandlers
{
    public class ObservationRepliedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ObservationRepliedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public ObservationRepliedEventHandler(
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

        public async Task Handle(DomainEventNotification<ObservationRepliedEvent> notification, CancellationToken token)
        {
            var @event = notification.DomainEvent;
            var observation = await _context.Set<Domain.Observation>().SingleAsync(e => e.Id == @event.ObservationId, token);
            var reply = await _context.Set<Domain.Observation>().SingleAsync(e => e.Id == @event.ReplyId, token);
            if (reply.User.Kind == ProfileKind.Producer && observation.User.Kind == ProfileKind.Producer)
                return;

            var targets = new List<Tuple<Guid, string, string>>();
            if (reply.User.Kind != ProfileKind.Producer)
                targets.Add(new Tuple<Guid, string, string>(observation.Producer.Id, observation.Producer.Name, observation.Producer.Email));
            else
            {
                targets.Add(new Tuple<Guid, string, string>(observation.User.Id, observation.User.Name, observation.User.Email));
                targets.AddRange(observation.Replies.Where(r => r.UserId != observation.ProducerId).Select(r =>
                    new Tuple<Guid, string, string>(r.User.Id, r.User.Name, r.User.Email)));
                
                targets = targets.Distinct().ToList();
            }

            foreach (var target in targets)
            {
                var observationId = _idSerializer.Serialize("Query", nameof(Observation), observation.Id);
                var producerId = _idSerializer.Serialize("Query", nameof(Producer), observation.ProducerId);
                
                var url = reply.User.Kind == ProfileKind.Producer
                    ? $"{_configuration.GetValue<string>("Portal:url")}/#/store-traceability/?observationId={observationId}&producerId={producerId}&refresh={Guid.NewGuid():N}"
                    : $"{_configuration.GetValue<string>("Portal:url")}/#/traceability/?observationId={observationId}&refresh={Guid.NewGuid():N}";
                
                await _signalrService.SendNotificationToUserAsync(target.Item1, nameof(ObservationRepliedEvent), reply.GetNotificationContent(observationId, url, producerId));
            
                await _emailService.SendTemplatedEmailAsync(
                    target.Item3,
                    target.Item2,
                    $"{reply.User.Name} a répondu à votre remarque",
                    nameof(ObservationRepliedEvent),
                    reply.GetNotificationData(observationId, url, reply.Comment, producerId),
                    true,
                    token);
            }
        }
    }
}