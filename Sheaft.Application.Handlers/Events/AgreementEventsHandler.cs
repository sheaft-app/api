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
        INotificationHandler<AgreementCreatedByStoreEvent>,
        INotificationHandler<AgreementCreatedByProducerEvent>,
        INotificationHandler<AgreementAcceptedByStoreEvent>,
        INotificationHandler<AgreementAcceptedByProducerEvent>,
        INotificationHandler<AgreementCancelledByStoreEvent>,
        INotificationHandler<AgreementCancelledByProducerEvent>,
        INotificationHandler<AgreementRefusedByStoreEvent>,
        INotificationHandler<AgreementRefusedByProducerEvent>
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

        public async Task Handle(AgreementCreatedByStoreEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Delivery.Producer.Id, nameof(AgreementCreatedByStoreEvent), GetNotificationContent(agreement));

            await _emailService.SendTemplatedEmailAsync(
                agreement.Delivery.Producer.Email, 
                agreement.Delivery.Producer.Name, 
                AgreementCreatedByStoreEvent.MAILING_TEMPLATE_ID, 
                GetNotificationDatas(agreement), 
                token);
        }

        public async Task Handle(AgreementCreatedByProducerEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Store.Id, nameof(AgreementCreatedByProducerEvent), GetNotificationContent(agreement));

            await _emailService.SendTemplatedEmailAsync(
                agreement.Store.Email,
                agreement.Store.Name,
                AgreementCreatedByProducerEvent.MAILING_TEMPLATE_ID,
                GetNotificationDatas(agreement),
                token); 
        }

        public async Task Handle(AgreementAcceptedByStoreEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Delivery.Producer.Id, nameof(AgreementAcceptedByStoreEvent), GetNotificationContent(agreement));

            await _emailService.SendTemplatedEmailAsync(
                agreement.Delivery.Producer.Email,
                agreement.Delivery.Producer.Name,
                AgreementAcceptedByStoreEvent.MAILING_TEMPLATE_ID,
                GetNotificationDatas(agreement),
                token);
        }

        public async Task Handle(AgreementAcceptedByProducerEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Store.Id, nameof(AgreementAcceptedByProducerEvent), GetNotificationContent(agreement));

            await _emailService.SendTemplatedEmailAsync(
                agreement.Store.Email,
                agreement.Store.Name,
                AgreementAcceptedByProducerEvent.MAILING_TEMPLATE_ID,
                GetNotificationDatas(agreement),
                token);
        }

        public async Task Handle(AgreementCancelledByStoreEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Delivery.Producer.Id, nameof(AgreementCancelledByStoreEvent), GetNotificationContent(agreement));

            await _emailService.SendTemplatedEmailAsync(
                agreement.Delivery.Producer.Email,
                agreement.Delivery.Producer.Name,
                AgreementCancelledByStoreEvent.MAILING_TEMPLATE_ID,
                GetNotificationDatas(agreement),
                token);
        }
        
        public async Task Handle(AgreementCancelledByProducerEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Delivery.Producer.Id, nameof(AgreementCancelledByProducerEvent), GetNotificationContent(agreement));

            await _emailService.SendTemplatedEmailAsync(
                agreement.Store.Email,
                agreement.Store.Name,
                AgreementCancelledByProducerEvent.MAILING_TEMPLATE_ID,
                GetNotificationDatas(agreement),
                token);
        }

        public async Task Handle(AgreementRefusedByStoreEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Delivery.Producer.Id, nameof(AgreementRefusedByStoreEvent), GetNotificationContent(agreement));
            await _emailService.SendTemplatedEmailAsync(
                agreement.Delivery.Producer.Email,
                agreement.Delivery.Producer.Name,
                AgreementRefusedByStoreEvent.MAILING_TEMPLATE_ID,
                GetNotificationDatas(agreement),
                token);
        }

        public async Task Handle(AgreementRefusedByProducerEvent agreementEvent, CancellationToken token)
        {
            var agreement = await _context.GetByIdAsync<Agreement>(agreementEvent.Id, token);
            await _signalrService.SendNotificationToGroupAsync(agreement.Delivery.Producer.Id, nameof(AgreementRefusedByProducerEvent), GetNotificationContent(agreement));

            await _emailService.SendTemplatedEmailAsync(
                agreement.Store.Email,
                agreement.Store.Name,
                AgreementRefusedByProducerEvent.MAILING_TEMPLATE_ID,
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