using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Core.Extensions;

namespace Sheaft.Functions
{
    public class DocumentFunctions
    {
        private readonly IMediator _mediatr;

        public DocumentFunctions(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("SetDocumentStatusCommand")]
        public async Task SetDocumentStatusCommandAsync([ServiceBusTrigger(SetDocumentStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetDocumentStatusCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("DocumentFailedEvent")]
        public async Task DocumentFailedEventAsync([ServiceBusTrigger(DocumentFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<DocumentFailedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.DocumentId.ToString("N"));
        }

        [FunctionName("DocumentOutdatedEvent")]
        public async Task DocumentOutdatedEventAsync([ServiceBusTrigger(DocumentOutdatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<DocumentOutdatedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.DocumentId.ToString("N"));
        }

        [FunctionName("DocumentSucceededEvent")]
        public async Task DocumentSucceededEventAsync([ServiceBusTrigger(DocumentSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<DocumentSucceededEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.DocumentId.ToString("N"));
        }
    }
}
