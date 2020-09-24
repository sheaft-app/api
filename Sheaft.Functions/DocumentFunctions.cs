using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Functions
{
    public class DocumentFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public DocumentFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("SetDocumentStatusCommand")]
        public async Task SetDocumentStatusCommandAsync([ServiceBusTrigger(SetDocumentStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<SetDocumentStatusCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("DocumentFailedEvent")]
        public async Task DocumentFailedEventAsync([ServiceBusTrigger(DocumentFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<DocumentFailedEvent>(message, token);
        }

        [FunctionName("DocumentOutdatedEvent")]
        public async Task DocumentOutdatedEventAsync([ServiceBusTrigger(DocumentOutdatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<DocumentOutdatedEvent>(message, token);
        }

        [FunctionName("DocumentSucceededEvent")]
        public async Task DocumentSucceededEventAsync([ServiceBusTrigger(DocumentSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<DocumentSucceededEvent>(message, token);
        }
    }
}
