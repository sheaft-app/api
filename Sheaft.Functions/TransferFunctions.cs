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
    public class TransferFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public TransferFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("CheckTransferTransactionsCommand")]
        public async Task CheckPayinTransactionsCommandAsync([TimerTrigger("0 */10 * * * *", RunOnStartup = false)] TimerInfo info, CancellationToken token)
        {
            var results = await _mediatr.Process(new CheckTransferTransactionsCommand(new RequestUser("transfer-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetTransferStatusCommand")]
        public async Task SetTransferStatusCommandAsync([ServiceBusTrigger(SetTransferStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<SetTransferStatusCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetRefundTransferStatusCommand")]
        public async Task SetRefundTransferStatusCommandAsync([ServiceBusTrigger(SetRefundTransferStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<SetRefundTransferStatusCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckCreatedTransferTransactionCommand")]
        public async Task CheckCreatedTransferTransactionCommandAsync([ServiceBusTrigger(CheckCreatedTransferTransactionCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CheckCreatedTransferTransactionCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckWaitingTransferTransactionCommand")]
        public async Task CheckWaitingTransferTransactionCommandAsync([ServiceBusTrigger(CheckWaitingTransferTransactionCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CheckWaitingTransferTransactionCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("TransferFailedEvent")]
        public async Task TransferFailedEventAsync([ServiceBusTrigger(TransferFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<TransferFailedEvent>(message, token);
        }

        [FunctionName("TransferSucceededEvent")]
        public async Task TransferSucceededEventAsync([ServiceBusTrigger(TransferSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<TransferSucceededEvent>(message, token);
        }

        [FunctionName("RefundTransferFailedEvent")]
        public async Task RefundTransferFailedEventAsync([ServiceBusTrigger(RefundTransferFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<RefundTransferFailedEvent>(message, token);
        }

        [FunctionName("RefundTransferSucceededEvent")]
        public async Task RefundTransferSucceededEventAsync([ServiceBusTrigger(RefundTransferSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<RefundTransferSucceededEvent>(message, token);
        }
    }
}
