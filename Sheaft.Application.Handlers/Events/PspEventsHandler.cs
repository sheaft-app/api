using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Events;
using Sheaft.Interop.Enums;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{
    public class PspEventsHandler :
        INotificationHandler<NewPspHookEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly IQueueService _queueService;

        public PspEventsHandler(IConfiguration configuration, IQueueService queueService)
        {
            _configuration = configuration;
            _queueService = queueService;
        }

        public async Task Handle(NewPspHookEvent hookEvent, CancellationToken token)
        {
            switch (hookEvent.Kind)
            {
                case PspEventKind.KYC_SUCCEEDED:
                    break;
                case PspEventKind.KYC_FAILED:
                    break;
                case PspEventKind.KYC_OUTDATED:
                    break;
                case PspEventKind.UBO_DECLARATION_REFUSED:
                    break;
                case PspEventKind.UBO_DECLARATION_VALIDATED:
                    break;
                case PspEventKind.UBO_DECLARATION_INCOMPLETE:
                    break;
                case PspEventKind.PAYIN_NORMAL_SUCCEEDED:
                    break;
                case PspEventKind.PAYIN_NORMAL_FAILED:
                    break;
                case PspEventKind.TRANSFER_NORMAL_SUCCEEDED:
                    break;
                case PspEventKind.TRANSFER_NORMAL_FAILED:
                    break;
                case PspEventKind.PAYOUT_NORMAL_SUCCEEDED:
                    break;
                case PspEventKind.PAYOUT_NORMAL_FAILED:
                    break;
                case PspEventKind.PAYIN_REFUND_SUCCEEDED:
                    break;
                case PspEventKind.PAYIN_REFUND_FAILED:
                    break;
                case PspEventKind.TRANSFER_REFUND_SUCCEEDED:
                    break;
                case PspEventKind.TRANSFER_REFUND_FAILED:
                    break;
                case PspEventKind.USER_KYC_REGULAR:
                case PspEventKind.USER_KYC_LIGHT:
                case PspEventKind.PAYOUT_REFUND_SUCCEEDED:
                case PspEventKind.PAYOUT_REFUND_FAILED:
                case PspEventKind.PAYIN_NORMAL_CREATED:
                case PspEventKind.PAYOUT_NORMAL_CREATED:
                case PspEventKind.TRANSFER_NORMAL_CREATED:
                case PspEventKind.PAYIN_REFUND_CREATED:
                case PspEventKind.PAYOUT_REFUND_CREATED:
                case PspEventKind.TRANSFER_REFUND_CREATED:
                case PspEventKind.UBO_DECLARATION_CREATED:
                case PspEventKind.UBO_DECLARATION_VALIDATION_ASKED:
                case PspEventKind.PAYIN_REPUDIATION_CREATED:
                case PspEventKind.PAYIN_REPUDIATION_SUCCEEDED:
                case PspEventKind.PAYIN_REPUDIATION_FAILED:
                case PspEventKind.KYC_CREATED:
                case PspEventKind.KYC_VALIDATION_ASKED:
                case PspEventKind.DISPUTE_DOCUMENT_CREATED:
                case PspEventKind.DISPUTE_DOCUMENT_VALIDATION_ASKED:
                case PspEventKind.DISPUTE_DOCUMENT_SUCCEEDED:
                case PspEventKind.DISPUTE_DOCUMENT_FAILED:
                case PspEventKind.DISPUTE_CREATED:
                case PspEventKind.DISPUTE_SUBMITTED:
                case PspEventKind.DISPUTE_ACTION_REQUIRED:
                case PspEventKind.DISPUTE_FURTHER_ACTION_REQUIRED:
                case PspEventKind.DISPUTE_CLOSED:
                case PspEventKind.DISPUTE_SENT_TO_BANK:
                case PspEventKind.TRANSFER_SETTLEMENT_CREATED:
                case PspEventKind.TRANSFER_SETTLEMENT_SUCCEEDED:
                case PspEventKind.TRANSFER_SETTLEMENT_FAILED:
                case PspEventKind.MANDATE_CREATED:
                case PspEventKind.MANDATE_FAILED:
                case PspEventKind.MANDATE_ACTIVATED:
                case PspEventKind.MANDATE_SUBMITTED:
                case PspEventKind.MANDATE_EXPIRED:
                case PspEventKind.PREAUTHORIZATION_PAYMENT_WAITING:
                case PspEventKind.PREAUTHORIZATION_PAYMENT_EXPIRED:
                case PspEventKind.PREAUTHORIZATION_PAYMENT_CANCELED:
                case PspEventKind.PREAUTHORIZATION_PAYMENT_VALIDATED:
                default:
                    break;
            }
        }
    }
}