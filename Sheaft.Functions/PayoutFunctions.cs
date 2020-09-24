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
    public class PayoutFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public PayoutFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("CheckPayoutTransactionsCommand")]
        public async Task CheckPayoutTransactionsCommandAsync([TimerTrigger("0 * * */1 * *", RunOnStartup = false)] TimerInfo info, CancellationToken token)
        {
            var results = await _mediatr.Process(new CheckPayoutTransactionsCommand(new RequestUser("payout-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckForNewPayoutsCommand")]
        public async Task CheckForNewPayoutsCommandAsync([TimerTrigger("0 */10 * * * *", RunOnStartup = false)] TimerInfo info, CancellationToken token)
        {
            var results = await _mediatr.Process(new CheckForNewPayoutsCommand(new RequestUser("payout-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CreatePayoutForTransfersCommand")]
        public async Task CreatePayoutForTransfersCommandAsync([ServiceBusTrigger(CreatePayoutForTransfersCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CreatePayoutForTransfersCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckCreatedPayoutTransactionCommand")]
        public async Task CheckCreatedPayoutTransactionCommandAsync([ServiceBusTrigger(CheckCreatedPayoutTransactionCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CheckCreatedPayoutTransactionCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckWaitingPayoutTransactionCommand")]
        public async Task CheckWaitingPayoutTransactionCommandAsync([ServiceBusTrigger(CheckWaitingPayoutTransactionCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CheckWaitingPayoutTransactionCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetPayoutStatusCommand")]
        public async Task SetPayoutStatusCommandAsync([ServiceBusTrigger(SetPayoutStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<SetPayoutStatusCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetRefundPayoutStatusCommand")]
        public async Task SetRefundPayoutStatusCommandAsync([ServiceBusTrigger(SetRefundPayoutStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<SetRefundPayoutStatusCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("PayoutFailedEvent")]
        public async Task PayoutFailedEventAsync([ServiceBusTrigger(PayoutFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PayoutFailedEvent>(message, token);
        }

        [FunctionName("PayoutSucceededEvent")]
        public async Task PayoutSucceededEventAsync([ServiceBusTrigger(PayoutSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PayoutSucceededEvent>(message, token);
        }
    }
}
