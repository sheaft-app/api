using System;
using System.Collections.Generic;
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
    public class OrderFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public OrderFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("CheckOrdersCommand")]
        public async Task CheckOrdersCommandAsync([TimerTrigger("0 */10 * * * *", RunOnStartup = false)] TimerInfo info, CancellationToken token)
        {
            var results = await _mediatr.Process(new CheckOrdersCommand(new RequestUser("orders-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CheckOrderCommand")]
        public async Task CheckOrderCommandAsync([ServiceBusTrigger(CheckOrderCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CheckOrderCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("ConfirmOrderCommand")]
        public async Task ConfirmOrderCommandAsync([ServiceBusTrigger(ConfirmOrderCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<ConfirmOrderCommand, IEnumerable<Guid>>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("FailOrderCommand")]
        public async Task FailOrderCommandAsync([ServiceBusTrigger(FailOrderCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<FailOrderCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("ExpireOrderCommand")]
        public async Task ExpireOrderCommandAsync([ServiceBusTrigger(ExpireOrderCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<ExpireOrderCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("UnblockOrderCommand")]
        public async Task UnblockOrderCommandAsync([ServiceBusTrigger(UnblockOrderCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<UnblockOrderCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }
    }
}
