﻿using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Agreement;
using Sheaft.Mailing.Extensions;

namespace Sheaft.Mediatr.Agreement.EventHandlers
{
    public class AgreementRefuseEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<AgreementRefusedEvent>>
    {
        private readonly IConfiguration _configuration;
        private readonly IIdSerializer _idSerializer;

        public AgreementRefuseEventHandler(
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

        public async Task Handle(DomainEventNotification<AgreementRefusedEvent> notification, CancellationToken token)
        {
            var agreementEvent = notification.DomainEvent;
            var agreement = await _context.Agreements.SingleAsync(e => e.Id == agreementEvent.AgreementId, token);

            var email = string.Empty;
            var name = string.Empty;
            var targetName = string.Empty;
            var id = Guid.Empty;

            var subEventName = string.Empty;
            if (agreementEvent.RequestedByKind == ProfileKind.Producer)
            {
                email = agreement.Producer.Email;
                name = agreement.Producer.Name;
                targetName = agreement.Store.Name;
                id = agreement.Producer.Id;
                subEventName = "ByStore";
            }
            else
            {
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                targetName = agreement.Producer.Name;
                id = agreement.Store.Id;
                subEventName = "ByProducer";
            }

            var eventName = nameof(AgreementRefusedEvent).Replace("Event", $"{subEventName}Event");
            await _signalrService.SendNotificationToUserAsync(id, eventName, agreement.GetNotificationContent(_idSerializer.Serialize("Query", nameof(Agreement), agreement.Id), _configuration.GetValue<string>("Portal:url"), targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                $"{targetName} a refusé votre demande de partenariat",
                nameof(AgreementRefusedEvent),
                agreement.GetNotificationData(_idSerializer.Serialize("Query", nameof(Agreement), agreement.Id), _configuration.GetValue<string>("Portal:url"), targetName),
                true,
                token);
        }
    }
}