using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using System;
using Sheaft.Application.Models.Mailer;

namespace Sheaft.Application.Handlers
{
    public class AgreementEventsHandler : EventsHandler,
        INotificationHandler<AgreementCreatedEvent>,
        INotificationHandler<AgreementAcceptedEvent>,
        INotificationHandler<AgreementCancelledEvent>,
        INotificationHandler<AgreementRefusedEvent>
    {
        private readonly IConfiguration _configuration;

        public AgreementEventsHandler(
            IConfiguration configuration,
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
            _configuration = configuration;
        }

        public async Task Handle(AgreementCreatedEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);
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
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement, targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                $"{targetName} souhaiterait commercer avec vous",
                nameof(AgreementCreatedEvent),
                GetNotificationDatas(agreement, targetName),
                true,
                token);
        }

        public async Task Handle(AgreementAcceptedEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);

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
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement, targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                $"{targetName} a accepté de commercer avec vous",
                nameof(AgreementAcceptedEvent),
                GetNotificationDatas(agreement, targetName),
                true,
                token);
        }

        public async Task Handle(AgreementCancelledEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);

            var email = string.Empty;
            var name = string.Empty;
            var targetName = string.Empty;
            var id = Guid.Empty;

            var subEventName = string.Empty;
            if (agreementEvent.RequestUser.Id == agreement.Delivery.Producer.Id)
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

            var eventName = nameof(AgreementCancelledEvent).Replace("Event", $"{subEventName}Event");
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement, targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                $"{targetName} a annulé votre accord commercial",
                nameof(AgreementCancelledEvent),
                GetNotificationDatas(agreement, targetName),
                true,
                token);
        }

        public async Task Handle(AgreementRefusedEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);

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

            var eventName = nameof(AgreementRefusedEvent).Replace("Event", $"{subEventName}Event");
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement, targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                $"{targetName} a refusé votre demande d'accord commercial",
                nameof(AgreementRefusedEvent),
                GetNotificationDatas(agreement, targetName),
                true,
                token);
        }

        private StringContent GetNotificationContent(Domain.Models.Agreement agreement, string name)
        {
            return new StringContent(JsonConvert.SerializeObject(GetNotificationDatas(agreement, name)), Encoding.UTF8, "application/json");
        }

        private AgreementMailerModel GetNotificationDatas(Domain.Models.Agreement agreement, string name)
        {
            return new AgreementMailerModel
            { 
                Name = name,
                Reason = agreement.Reason,
                AgreementId = agreement.Id, 
                CreatedOn = agreement.CreatedOn, 
                PortalUrl = $"{_configuration.GetValue<string>("Portal:url")}/#/agreements/{agreement.Id}" 
            };
        }
    }
}