using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;

namespace Sheaft.Functions
{
    public class PayinRefundFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public PayinRefundFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("RefreshPayinRefundStatusCommand")]
        public async Task RefreshPayinRefundStatusCommandAsync([ServiceBusTrigger(RefreshPayinRefundStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RefreshPayinRefundStatusCommand, TransactionStatus>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("PayinRefundFailedEvent")]
        public async Task PayinRefundFailedEventAsync([ServiceBusTrigger(PayinRefundFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PayinRefundFailedEvent>(message, token);
        }

        [FunctionName("PayinRefundSucceededEvent")]
        public async Task PayinRefundSucceededEventAsync([ServiceBusTrigger(PayinRefundSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PayinRefundSucceededEvent>(message, token);
        }
    }
}
