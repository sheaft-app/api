using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Logging;

namespace Sheaft.Functions
{
    public class NotificationFunctions
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediatr;

        public NotificationFunctions(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediatr = mediator;
        }

        [FunctionName("CreateUserNotificationCommand")]
        public async Task CreateUserNotificationCommandAsync([QueueTrigger(CreateUserNotificationCommand.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<CreateUserNotificationCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("CreateGroupNotificationCommand")]
        public async Task CreateGroupNotificationCommandAsync([QueueTrigger(CreateGroupNotificationCommand.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<CreateGroupNotificationCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }
    }
}
