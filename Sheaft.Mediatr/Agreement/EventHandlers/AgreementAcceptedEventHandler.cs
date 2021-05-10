using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Agreement;

namespace Sheaft.Mediatr.Agreement.EventHandlers
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
            var agreement = await _context.Agreements.SingleAsync(e => e.Id == agreementEvent.AgreementId, token);

            var email = string.Empty;
            var name = string.Empty;
            var targetName = string.Empty;
            var id = Guid.Empty;

            var subEventName = string.Empty;
            if (agreementEvent.RequestedByKind == ProfileKind.Producer)
            {
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                targetName = agreement.Producer.Name;
                id = agreement.Store.Id;
                subEventName = "ByProducer";
            }
            else
            {
                email = agreement.Producer.Email;
                name = agreement.Producer.Name;
                targetName = agreement.Store.Name;
                id = agreement.Producer.Id;
                subEventName = "ByStore";
            }

            var eventName = nameof(AgreementAcceptedEvent).Replace("Event", $"{subEventName}Event");
            await _signalrService.SendNotificationToGroupAsync(id, eventName, agreement.GetNotificationContent(_configuration, targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                $"{targetName} a accepté de commercer avec vous",
                nameof(AgreementAcceptedEvent),
                agreement.GetNotificationData(_configuration, targetName),
                true,
                token);
        }
    }
}