﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Services;
using Sheaft.Application.Common.Mediatr;
using Sheaft.Application.Common.Models;
using Sheaft.Domain.Events.Agreement;

namespace Sheaft.Application.Agreement.EventHandlers
{
    public class AgreementAcceptedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<AgreementAcceptedEvent>>
    {
        private readonly IConfiguration _configuration;

        public AgreementAcceptedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<AgreementAcceptedEvent> notification, CancellationToken token)
        {
            var agreementEvent = notification.DomainEvent;
            var agreement = await _context.GetByIdAsync<Domain.Agreement>(agreementEvent.AgreementId, token);

            var email = string.Empty;
            var name = string.Empty;
            var targetName = string.Empty;
            var id = Guid.Empty;

            var subEventName = string.Empty;
            if (agreement.CreatedBy.Id == agreement.Delivery.Producer.Id)
            {
                email = agreement.Delivery.Producer.Email;
                name = agreement.Delivery.Producer.Name;
                targetName = agreement.Store.Name;
                id = agreement.Delivery.Producer.Id;
                subEventName = "ByStore";
            }
            else
            {
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                targetName = agreement.Delivery.Producer.Name;
                id = agreement.Store.Id;
                subEventName = "ByProducer";
            }

            var eventName = nameof(AgreementAcceptedEvent).Replace("Event", $"{subEventName}Event");
            await _signalrService.SendNotificationToGroupAsync(id, eventName, agreement.GetNotificationContent(_configuration, targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                $"{targetName} a accepté de commercer avec vous",
                nameof(AgreementAcceptedEvent),
                agreement.GetNotificationDatas(_configuration, targetName),
                true,
                token);
        }
    }
}