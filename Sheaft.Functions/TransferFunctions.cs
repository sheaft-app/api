using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Core;
using Sheaft.Logging;

namespace Sheaft.Functions
{
    public class TransferFunctions
    {
        private readonly IMediator _mediatr;

        public TransferFunctions(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("SetTransferStatusCommand")]
        public async Task SetTransferStatusCommandAsync([ServiceBusTrigger(SetTransferStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetTransferStatusCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetRefundTransferStatusCommand")]
        public async Task SetRefundTransferStatusCommandAsync([ServiceBusTrigger(SetRefundTransferStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetRefundTransferStatusCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckTransferTransactionsCommand")]
        public async Task CheckPayinTransactionsCommandAsync([TimerTrigger("0 */10 * * * *", RunOnStartup = false)] TimerInfo info, ILogger logger, CancellationToken token)
        {
            var results = await _mediatr.Send(new CheckTransferTransactionsCommand(new RequestUser("transfer-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;

            logger.LogInformation(nameof(CheckPayinTransactionsCommandAsync), "successfully executed");
        }

        [FunctionName("CheckCreatedTransferTransactionCommand")]
        public async Task CheckCreatedTransferTransactionCommandAsync([ServiceBusTrigger(CheckCreatedTransferTransactionCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<CheckCreatedTransferTransactionCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckWaitingTransferTransactionCommand")]
        public async Task CheckWaitingTransferTransactionCommandAsync([ServiceBusTrigger(CheckWaitingTransferTransactionCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<CheckWaitingTransferTransactionCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("TransferFailedEvent")]
        public async Task TransferFailedEventAsync([ServiceBusTrigger(TransferFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<TransferFailedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.TransactionId.ToString("N"));
        }

        [FunctionName("TransferSucceededEvent")]
        public async Task TransferSucceededEventAsync([ServiceBusTrigger(TransferSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<TransferSucceededEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.TransactionId.ToString("N"));
        }

        [FunctionName("RefundTransferFailedEvent")]
        public async Task RefundTransferFailedEventAsync([ServiceBusTrigger(RefundTransferFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<RefundTransferFailedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.TransactionId.ToString("N"));
        }

        [FunctionName("RefundTransferSucceededEvent")]
        public async Task RefundTransferSucceededEventAsync([ServiceBusTrigger(RefundTransferSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<RefundTransferSucceededEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.TransactionId.ToString("N"));
        }
    }
}
