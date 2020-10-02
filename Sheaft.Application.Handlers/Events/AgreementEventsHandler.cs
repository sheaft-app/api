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
            await _signalrService.SendNotificationToGroupAsync(agreement.Delivery.Producer.Id, nameof(AgreementCreatedEvent), GetNotificationContent(agreement));

            await _emailService.SendTemplatedEmailAsync(
                agreement.Delivery.Producer.Email,
                agreement.Delivery.Producer.Name,
                _emailTemplateOptions.AgreementCreatedEvent,
                GetNotificationDatas(agreement),
                token);
        }

        public async Task Handle(AgreementAcceptedEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Delivery.Producer.Id, nameof(AgreementAcceptedEvent), GetNotificationContent(agreement));

            await _emailService.SendTemplatedEmailAsync(
                agreement.Delivery.Producer.Email,
                agreement.Delivery.Producer.Name,
                _emailTemplateOptions.AgreementAcceptedEvent,
                GetNotificationDatas(agreement),
                token);
        }

        public async Task Handle(AgreementCancelledEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Delivery.Producer.Id, nameof(AgreementCancelledEvent), GetNotificationContent(agreement));

            await _emailService.SendTemplatedEmailAsync(
                agreement.Delivery.Producer.Email,
                agreement.Delivery.Producer.Name,
                _emailTemplateOptions.AgreementCancelledEvent,
                GetNotificationDatas(agreement),
                token);
        }

        public async Task Handle(AgreementRefusedEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Delivery.Producer.Id, nameof(AgreementRefusedEvent), GetNotificationContent(agreement));
            await _emailService.SendTemplatedEmailAsync(
                agreement.Delivery.Producer.Email,
                agreement.Delivery.Producer.Name,
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
            return new { StoreName = agreement.Store.Name, ProducerName = agreement.Delivery.Producer.Name, AgreementId = agreement.Id, CreatedOn = agreement.CreatedOn, PortalUrl = $"{_configuration.GetValue<string>("Urls:Portal")}/#/agreements/{agreement.Id}" };
        }
    }
}