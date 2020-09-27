using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Commands;
using Sheaft.Application.Interop;

namespace Sheaft.Functions
{
    public class NotificationFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public NotificationFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("CreateUserNotificationCommand")]
        public async Task CreateUserNotificationCommandAsync([ServiceBusTrigger(CreateUserNotificationCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CreateUserNotificationCommand, Guid>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CreateGroupNotificationCommand")]
        public async Task CreateGroupNotificationCommandAsync([ServiceBusTrigger(CreateGroupNotificationCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CreateGroupNotificationCommand, Guid>(message, token);
            if (!results.Success)
                throw results.Exception;
        }
    }
}
