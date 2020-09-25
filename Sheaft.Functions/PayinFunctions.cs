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

        [FunctionName("CheckPayinsCommand")]
        public async Task CheckPayinsCommandAsync([TimerTrigger("0 */10 * * * *", RunOnStartup = false)] TimerInfo info, CancellationToken token)
        {
            var results = await _mediatr.Process(new CheckPayinsCommand(new RequestUser("payin-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckPayinCommand")]
        public async Task CheckPayinCommandAsync([ServiceBusTrigger(CheckPayinCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CheckPayinCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("RefreshPayinStatusCommand")]
        public async Task RefreshPayinStatusCommandAsync([ServiceBusTrigger(RefreshPayinStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RefreshPayinStatusCommand, bool>(message, token);
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
    }
}
