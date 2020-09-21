using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Core.Extensions;

namespace Sheaft.Functions
{
    public class UserFunctions
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediatr;

        public UserFunctions(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediatr = mediator;
        }

        [FunctionName("ExportUserDataCommand")]
        public async Task ExportUserDataCommandAsync([ServiceBusTrigger(ExportUserDataCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<ExportUserDataCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("RemoveUserDataCommand")]
        public async Task RemoveUserDataCommandAsync([ServiceBusTrigger(RemoveUserDataCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<RemoveUserDataCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }


        [FunctionName("CreateUserPointsCommand")]
        public async Task CreateUserPointsCommandAsync([ServiceBusTrigger(CreateUserPointsCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<CreateUserPointsCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("ExportUserDataSucceededEvent")]
        public async Task ExportUserDataSucceededEventAsync([ServiceBusTrigger(ExportUserDataSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<ExportUserDataSucceededEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }

        [FunctionName("ExportUserDataFailedEvent")]
        public async Task ExportUserDataFailedEventAsync([ServiceBusTrigger(ExportUserDataFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<ExportUserDataFailedEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }

        [FunctionName("ExportUserDataProcessingEvent")]
        public async Task ExportUserDataProcessingEventAsync([ServiceBusTrigger(ExportUserDataProcessingEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<ExportUserDataProcessingEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }
    }
}
