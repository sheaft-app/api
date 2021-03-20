using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Domain.Events.Agreement;

namespace Sheaft.Mediatr.Agreement.EventHandlers
{
    public class AgreementCreatedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<AgreementCreatedEvent>>
    {
        private readonly IConfiguration _configuration;

        public AgreementCreatedEventHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(DomainEventNotification<AgreementCreatedEvent> notification, CancellationToken token)
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
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                targetName = agreement.Delivery.Producer.Name;
                id = agreement.Store.Id;
                subEventName = "ByProducer";
            }
            else
            {
                email = agreement.Delivery.Producer.Email;
                name = agreement.Delivery.Producer.Name;
                targetName = agreement.Store.Name;
                id = agreement.Delivery.Producer.Id;
                subEventName = "ByStore";
            }

            var eventName = nameof(AgreementCreatedEvent).Replace("Event", $"{subEventName}Event");
            await _signalrService.SendNotificationToGroupAsync(id, eventName, agreement.GetNotificationContent(_configuration, targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                $"{targetName} souhaiterait commercer avec vous",
                nameof(AgreementCreatedEvent),
                agreement.GetNotificationContent(_configuration, targetName),
                true,
                token);
        }
    }
}