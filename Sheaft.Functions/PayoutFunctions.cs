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
    public class PayoutFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public PayoutFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("CheckPayoutsCommand")]
        public async Task CheckPayoutsCommandAsync([TimerTrigger("0 * * */1 * *", RunOnStartup = false)] TimerInfo info, CancellationToken token)
        {
            var results = await _mediatr.Process(new CheckPayoutsCommand(new RequestUser("payout-functions", Guid.NewGuid().ToString("N"))), token);
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

        [FunctionName("CreatePayoutCommand")]
        public async Task CreatePayoutCommandAsync([ServiceBusTrigger(CreatePayoutCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CreatePayoutCommand, Guid>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckPayoutCommand")]
        public async Task CheckPayoutCommandAsync([ServiceBusTrigger(CheckPayoutCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CheckPayoutCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("RefreshPayoutStatusCommand")]
        public async Task RefreshPayoutStatusCommandAsync([ServiceBusTrigger(RefreshPayoutStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RefreshPayoutStatusCommand, TransactionStatus>(message, token);
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
