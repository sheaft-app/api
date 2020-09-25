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

namespace Sheaft.Application.Handlers
{
    public class AgreementEventsHandler :
        INotificationHandler<AgreementCreatedEvent>,
        INotificationHandler<AgreementAcceptedEvent>,
        INotificationHandler<AgreementCancelledEvent>,
        INotificationHandler<AgreementRefusedEvent>
    {
        private readonly IAppDbContext _context;
        private readonly ISignalrService _signalrService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AgreementEventsHandler(IConfiguration configuration, IAppDbContext context, IEmailService emailService, ISignalrService signalrService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _signalrService = signalrService;
        }

        public async Task Handle(AgreementCreatedEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.AgreementId, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Delivery.Producer.Id, nameof(AgreementCreatedEvent), GetNotificationContent(agreement));

            await _emailService.SendTemplatedEmailAsync(
                agreement.Delivery.Producer.Email, 
                agreement.Delivery.Producer.Name, 
                AgreementCreatedEvent.MAILING_TEMPLATE_ID, 
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
                AgreementAcceptedEvent.MAILING_TEMPLATE_ID,
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
                AgreementCancelledEvent.MAILING_TEMPLATE_ID,
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
                AgreementRefusedEvent.MAILING_TEMPLATE_ID,
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