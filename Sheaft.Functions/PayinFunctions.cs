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
using Sheaft.Core.Extensions;

namespace Sheaft.Functions
{
    public class PayinFunctions
    {
        private readonly IMediator _mediatr;

        public PayinFunctions(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("SetPayinStatusCommand")]
        public async Task SetPayinStatusCommandAsync([ServiceBusTrigger(SetPayinStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetPayinStatusCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetRefundPayinStatusCommand")]
        public async Task SetRefundPayinStatusCommandAsync([ServiceBusTrigger(SetRefundPayinStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetRefundPayinStatusCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckPayinTransactionsCommand")]
        public async Task CheckPayinTransactionsCommandAsync([TimerTrigger("0 */10 * * * *", RunOnStartup = false)] TimerInfo info, ILogger logger, CancellationToken token)
        {
            var results = await _mediatr.Send(new CheckPayinTransactionsCommand(new RequestUser("payin-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;

            logger.LogInformation(nameof(CheckPayinTransactionsCommandAsync), "successfully executed");
        }

        [FunctionName("CheckWaitingPayinTransactionCommand")]
        public async Task CheckWaitingPayinTransactionCommandAsync([ServiceBusTrigger(CheckWaitingPayinTransactionCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<CheckWaitingPayinTransactionCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckCreatedPayinTransactionCommand")]
        public async Task CheckCreatedPayinTransactionCommandAsync([ServiceBusTrigger(CheckCreatedPayinTransactionCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<CheckCreatedPayinTransactionCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("PayinFailedEvent")]
        public async Task PayinFailedEventAsync([ServiceBusTrigger(PayinFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PayinFailedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.TransactionId.ToString("N"));
        }

        [FunctionName("PayinSucceededEvent")]
        public async Task PayinSucceededEventAsync([ServiceBusTrigger(PayinSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PayinSucceededEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.TransactionId.ToString("N"));
        }

        [FunctionName("RefundPayinFailedEvent")]
        public async Task RefundPayinFailedEventAsync([ServiceBusTrigger(RefundPayinFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<RefundPayinFailedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.TransactionId.ToString("N"));
        }

        [FunctionName("RefundPayinSucceededEvent")]
        public async Task RefundPayinSucceededEventAsync([ServiceBusTrigger(RefundPayinSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<RefundPayinSucceededEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.TransactionId.ToString("N"));
        }
    }
}
