using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;

namespace Sheaft.Functions
{
    public class PayoutRefundFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public PayoutRefundFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("RefreshPayoutRefundStatusCommand")]
        public async Task RefreshPayoutRefundStatusCommandAsync([ServiceBusTrigger(RefreshPayoutRefundStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RefreshPayoutRefundStatusCommand, TransactionStatus>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("PayoutRefundFailedEvent")]
        public async Task PayoutRefundFailedEventAsync([ServiceBusTrigger(PayoutRefundFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PayoutRefundFailedEvent>(message, token);
        }

        [FunctionName("PayoutRefundSucceededEvent")]
        public async Task PayoutRefundSucceededEventAsync([ServiceBusTrigger(PayoutRefundSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PayoutRefundSucceededEvent>(message, token);
        }
    }
}
