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
            var id = Guid.Empty;

            var subEventName = "Event";
            if (agreement.CreatedBy.Id == agreement.Delivery.Producer.Id)
            {
                email = agreement.Delivery.Producer.Email;
                name = agreement.Delivery.Producer.Name;
                id = agreement.Delivery.Producer.Id;
                subEventName = "ByProducer" + subEventName;
            }
            else
            {
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                id = agreement.Store.Id;
                subEventName = "BySender" + subEventName;
            }

            var eventName = nameof(AgreementCreatedEvent).Replace("Event", subEventName);
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                _emailTemplateOptions.AgreementCreatedEvent,
                GetNotificationDatas(agreement),
                token);
        }

        public async Task Handle(AgreementAcceptedEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);

            var email = string.Empty;
            var name = string.Empty;
            var id = Guid.Empty;

            var subEventName = "Event";
            if (agreement.CreatedBy.Id == agreement.Delivery.Producer.Id)
            {
                email = agreement.Delivery.Producer.Email;
                name = agreement.Delivery.Producer.Name;
                id = agreement.Delivery.Producer.Id;
                subEventName = "ByProducer" + subEventName;
            }
            else
            {
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                id = agreement.Store.Id;
                subEventName = "BySender" + subEventName;
            }

            var eventName = nameof(AgreementAcceptedEvent).Replace("Event", subEventName);
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                _emailTemplateOptions.AgreementAcceptedEvent,
                GetNotificationDatas(agreement),
                token);
        }

        public async Task Handle(AgreementCancelledEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);

            var email = string.Empty;
            var name = string.Empty;
            var id = Guid.Empty;

            var subEventName = "Event";
            if (agreement.CreatedBy.Id == agreement.Delivery.Producer.Id)
            {
                email = agreement.Delivery.Producer.Email;
                name = agreement.Delivery.Producer.Name;
                id = agreement.Delivery.Producer.Id;
                subEventName = "ByProducer" + subEventName;
            }
            else
            {
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                id = agreement.Store.Id;
                subEventName = "BySender" + subEventName;
            }

            var eventName = nameof(AgreementCancelledEvent).Replace("Event", subEventName);
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                _emailTemplateOptions.AgreementCancelledEvent,
                GetNotificationDatas(agreement),
                token);
        }

        public async Task Handle(AgreementRefusedEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);

            var email = string.Empty;
            var name = string.Empty;
            var id = Guid.Empty;

            var subEventName = "Event";
            if (agreement.CreatedBy.Id == agreement.Delivery.Producer.Id)
            {
                email = agreement.Delivery.Producer.Email;
                name = agreement.Delivery.Producer.Name;
                id = agreement.Delivery.Producer.Id;
                subEventName = "ByProducer" + subEventName;
            }
            else
            {
                email = agreement.Store.Email;
                name = agreement.Store.Name;
                id = agreement.Store.Id;
                subEventName = "BySender" + subEventName;
            }

            var eventName = nameof(AgreementRefusedEvent).Replace("Event", subEventName);
            await _signalrService.SendNotificationToGroupAsync(id, eventName, GetNotificationContent(agreement));
            await _emailService.SendTemplatedEmailAsync(
                email,
                name,
                _emailTemplateOptions.AgreementRefusedEvent,
                GetNotificationDatas(agreement),
                token);
        }

        private StringContent GetNotificationContent(Domain.Models.Agreement agreement)
        {
            return new StringContent(JsonConvert.SerializeObject(GetNotificationDatas(agreement)), Encoding.UTF8, "application/json");
        }

        private object GetNotificationDatas(Domain.Models.Agreement agreement)
        {
            return new { 
                StoreName = agreement.Store.Name, 
                ProducerName = agreement.Delivery.Producer.Name, 
                AgreementId = agreement.Id, 
                CreatedOn = agreement.CreatedOn, 
                PortalUrl = $"{_configuration.GetValue<string>("Urls:Portal")}/#/agreements/{agreement.Id}" };
        }
    }
}