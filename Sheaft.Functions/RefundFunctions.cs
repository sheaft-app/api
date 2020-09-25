using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Functions
{
    public class RefundFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public RefundFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("RefreshPayinRefundStatusCommand")]
        public async Task RefreshPayinRefundStatusCommandAsync([ServiceBusTrigger(RefreshPayinRefundStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RefreshPayinRefundStatusCommand, bool>(message, token);
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

        [FunctionName("RefreshPayoutRefundStatusCommand")]
        public async Task RefreshPayoutRefundStatusCommandAsync([ServiceBusTrigger(RefreshPayoutRefundStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RefreshPayoutRefundStatusCommand, bool>(message, token);
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

        [FunctionName("RefreshTransferRefundStatusCommand")]
        public async Task RefreshTransferRefundStatusCommandAsync([ServiceBusTrigger(RefreshTransferRefundStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RefreshPayoutRefundStatusCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("TransferRefundFailedEvent")]
        public async Task TransferRefundFailedEventAsync([ServiceBusTrigger(TransferRefundFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<TransferRefundFailedEvent>(message, token);
        }

        [FunctionName("TransferRefundSucceededEvent")]
        public async Task TransferRefundSucceededEventAsync([ServiceBusTrigger(TransferRefundSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<TransferRefundSucceededEvent>(message, token);
        }
    }
}
