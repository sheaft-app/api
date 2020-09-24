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
    public class PayinFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public PayinFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("SetPayinStatusCommand")]
        public async Task SetPayinStatusCommandAsync([ServiceBusTrigger(SetPayinStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<SetPayinStatusCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetRefundPayinStatusCommand")]
        public async Task SetRefundPayinStatusCommandAsync([ServiceBusTrigger(SetRefundPayinStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<SetRefundPayinStatusCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckPayinTransactionsCommand")]
        public async Task CheckPayinTransactionsCommandAsync([TimerTrigger("0 */10 * * * *", RunOnStartup = false)] TimerInfo info, CancellationToken token)
        {
            var results = await _mediatr.Process(new CheckPayinTransactionsCommand(new RequestUser("payin-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckWaitingPayinTransactionCommand")]
        public async Task CheckWaitingPayinTransactionCommandAsync([ServiceBusTrigger(CheckWaitingPayinTransactionCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CheckWaitingPayinTransactionCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckCreatedPayinTransactionCommand")]
        public async Task CheckCreatedPayinTransactionCommandAsync([ServiceBusTrigger(CheckCreatedPayinTransactionCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CheckCreatedPayinTransactionCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("PayinFailedEvent")]
        public async Task PayinFailedEventAsync([ServiceBusTrigger(PayinFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PayinFailedEvent>(message, token);
        }

        [FunctionName("PayinSucceededEvent")]
        public async Task PayinSucceededEventAsync([ServiceBusTrigger(PayinSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PayinSucceededEvent>(message, token);
        }

        [FunctionName("RefundPayinFailedEvent")]
        public async Task RefundPayinFailedEventAsync([ServiceBusTrigger(RefundPayinFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<RefundPayinFailedEvent>(message, token);
        }

        [FunctionName("RefundPayinSucceededEvent")]
        public async Task RefundPayinSucceededEventAsync([ServiceBusTrigger(RefundPayinSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<RefundPayinSucceededEvent>(message, token);
        }
    }
}
