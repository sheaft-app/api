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
    public class TransferFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public TransferFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("CheckTransfersCommand")]
        public async Task CheckTransfersCommandAsync([TimerTrigger("0 */10 * * * *", RunOnStartup = false)] TimerInfo info, CancellationToken token)
        {
            var results = await _mediatr.Process(new CheckTransfersCommand(new RequestUser("transfer-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckNewTransfersCommand")]
        public async Task CheckNewTransfersCommandAsync([TimerTrigger("0 * */1 * * *", RunOnStartup = false)] TimerInfo info, CancellationToken token)
        {
            var results = await _mediatr.Process(new CheckNewTransfersCommand(new RequestUser("transfer-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CreateTransferCommand")]
        public async Task CreateTransferCommandAsync([ServiceBusTrigger(CreateTransferCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CreateTransferCommand, Guid>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("RefreshTransferStatusCommand")]
        public async Task RefreshTransferStatusCommandAsync([ServiceBusTrigger(RefreshTransferStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RefreshTransferStatusCommand, TransactionStatus>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckTransferCommand")]
        public async Task CheckTransferCommandAsync([ServiceBusTrigger(CheckTransferCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CheckTransferCommand, bool>(message, token);
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

        [FunctionName("CreateTransferFailedEvent")]
        public async Task CreateTransferFailedEventAsync([ServiceBusTrigger(CreateTransferFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<CreateTransferFailedEvent>(message, token);
        }
    }
}
