﻿using System;
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

            var observationId = _idSerializer.Serialize("Query", nameof(Observation), observation.Id);
            var producerId = _idSerializer.Serialize("Query", nameof(Producer), observation.ProducerId);

            var url = $"{_configuration.GetValue<string>("Portal:url")}/#/traceability/?observationId={observationId}&refresh={Guid.NewGuid():N}";

            await _signalrService.SendNotificationToUserAsync(observation.ProducerId, nameof(ObservationAddedEvent),
                observation.GetNotificationContent(observationId, url, producerId));

            await _emailService.SendTemplatedEmailAsync(
                observation.Producer.Email,
                observation.Producer.Name,
                $"{observation.User.Name} a ajouté une observation concernant un de vos produits ou lot.",
                nameof(ObservationAddedEvent),
                observation.GetNotificationData(observationId, url, observation.Comment, producerId),
                true,
                token);
        }
    }
}