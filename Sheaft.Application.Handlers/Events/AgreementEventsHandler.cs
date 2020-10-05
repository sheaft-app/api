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
using Sheaft.Options;
using Microsoft.Extensions.Options;
using System;

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
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
            : base(context, emailService, signalrService, emailTemplateOptions)
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

            var subEventName = "Event";
            if (agreement.CreatedBy.Id == agreement.Delivery.Producer.Id)
            {
                email = agreement.Delivery.Producer.Email;
                name = agreement.Delivery.Producer.Name;
                targetName = agreement.Store.Name;
                id = agreement.Delivery.Producer.Id;
                subEventName = "ByProducer" + subEventName;
            }
            else
            {
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                targetName = agreement.Delivery.Producer.Name;
                id = agreement.Store.Id;
                subEventName = "BySender" + subEventName;
            }

            var eventName = nameof(AgreementCreatedEvent).Replace("Event", subEventName);
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement, targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                _emailTemplateOptions.AgreementCreatedEvent,
                GetNotificationDatas(agreement, targetName),
                token);
        }

        public async Task Handle(AgreementAcceptedEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);

            var email = string.Empty;
            var name = string.Empty;
            var targetName = string.Empty;
            var id = Guid.Empty;

            var subEventName = "Event";
            if (agreement.CreatedBy.Id == agreement.Delivery.Producer.Id)
            {
                email = agreement.Delivery.Producer.Email;
                name = agreement.Delivery.Producer.Name;
                targetName = agreement.Store.Name;
                id = agreement.Delivery.Producer.Id;
                subEventName = "ByProducer" + subEventName;
            }
            else
            {
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                targetName = agreement.Delivery.Producer.Name;
                id = agreement.Store.Id;
                subEventName = "BySender" + subEventName;
            }

            var eventName = nameof(AgreementAcceptedEvent).Replace("Event", subEventName);
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement, targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                _emailTemplateOptions.AgreementAcceptedEvent,
                GetNotificationDatas(agreement, targetName),
                token);
        }

        public async Task Handle(AgreementCancelledEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);

            var email = string.Empty;
            var name = string.Empty;
            var targetName = string.Empty;
            var id = Guid.Empty;

            var subEventName = "Event";
            if (agreement.CreatedBy.Id == agreement.Delivery.Producer.Id)
            {
                email = agreement.Delivery.Producer.Email;
                name = agreement.Delivery.Producer.Name;
                targetName = agreement.Store.Name;
                id = agreement.Delivery.Producer.Id;
                subEventName = "ByProducer" + subEventName;
            }
            else
            {
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                targetName = agreement.Delivery.Producer.Name;
                id = agreement.Store.Id;
                subEventName = "BySender" + subEventName;
            }

            var eventName = nameof(AgreementCancelledEvent).Replace("Event", subEventName);
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement, targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                _emailTemplateOptions.AgreementCancelledEvent,
                GetNotificationDatas(agreement, targetName),
                token);
        }

        public async Task Handle(AgreementRefusedEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);

            var email = string.Empty;
            var name = string.Empty;
            var targetName = string.Empty;
            var id = Guid.Empty;

            var subEventName = "Event";
            if (agreement.CreatedBy.Id == agreement.Delivery.Producer.Id)
            {
                email = agreement.Delivery.Producer.Email;
                name = agreement.Delivery.Producer.Name;
                targetName = agreement.Store.Name;
                id = agreement.Delivery.Producer.Id;
                subEventName = "ByProducer" + subEventName;
            }
            else
            {
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                targetName = agreement.Delivery.Producer.Name;
                id = agreement.Store.Id;
                subEventName = "BySender" + subEventName;
            }

            var eventName = nameof(AgreementRefusedEvent).Replace("Event", subEventName);
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement, targetName));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                _emailTemplateOptions.AgreementRefusedEvent,
                GetNotificationDatas(agreement, targetName),
                token);
        }

        private StringContent GetNotificationContent(Domain.Models.Agreement agreement, string name)
        {
            return new StringContent(JsonConvert.SerializeObject(GetNotificationDatas(agreement, name)), Encoding.UTF8, "application/json");
        }

        private object GetNotificationDatas(Domain.Models.Agreement agreement, string name)
        {
            return new { 
                Name = name, 
                AgreementId = agreement.Id, 
                CreatedOn = agreement.CreatedOn, 
                PortalUrl = $"{_configuration.GetValue<string>("Urls:Portal")}/#/agreements/{agreement.Id}" };
        }
    }
}