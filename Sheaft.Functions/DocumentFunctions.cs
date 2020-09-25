using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;

namespace Sheaft.Functions
{
    public class DocumentFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public DocumentFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("RefreshDocumentStatusCommand")]
        public async Task RefreshDocumentStatusCommandAsync([ServiceBusTrigger(RefreshDocumentStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RefreshDocumentStatusCommand, DocumentStatus>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("DocumentRefusedEvent")]
        public async Task DocumentRefusedEventAsync([ServiceBusTrigger(DocumentRefusedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<DocumentRefusedEvent>(message, token);
        }

        [FunctionName("DocumentOutdatedEvent")]
        public async Task DocumentOutdatedEventAsync([ServiceBusTrigger(DocumentOutdatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<DocumentOutdatedEvent>(message, token);
        }

        [FunctionName("DocumentValidatedEvent")]
        public async Task DocumentValidatedEventAsync([ServiceBusTrigger(DocumentValidatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<DocumentValidatedEvent>(message, token);
        }
    }
}
