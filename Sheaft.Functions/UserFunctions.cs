using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Functions
{
    public class UserFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public UserFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("ExportUserDataCommand")]
        public async Task ExportUserDataCommandAsync([ServiceBusTrigger(ExportUserDataCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<ExportUserDataCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("RemoveUserDataCommand")]
        public async Task RemoveUserDataCommandAsync([ServiceBusTrigger(RemoveUserDataCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RemoveUserDataCommand, string>(message, token);
            if (!results.Success)
                throw results.Exception;
        }


        [FunctionName("CreateUserPointsCommand")]
        public async Task CreateUserPointsCommandAsync([ServiceBusTrigger(CreateUserPointsCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<CreateUserPointsCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("ExportUserDataSucceededEvent")]
        public async Task ExportUserDataSucceededEventAsync([ServiceBusTrigger(ExportUserDataSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<ExportUserDataSucceededEvent>(message, token);
        }

        [FunctionName("ExportUserDataFailedEvent")]
        public async Task ExportUserDataFailedEventAsync([ServiceBusTrigger(ExportUserDataFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<ExportUserDataFailedEvent>(message, token);
        }

        [FunctionName("ExportUserDataProcessingEvent")]
        public async Task ExportUserDataProcessingEventAsync([ServiceBusTrigger(ExportUserDataProcessingEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<ExportUserDataProcessingEvent>(message, token);
        }
    }
}
