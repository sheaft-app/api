using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Functions
{
    public class ProducerFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public ProducerFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("ProducerDocumentsNotCreatedEvent")]
        public async Task ProducerDocumentsNotCreatedEventAsync([ServiceBusTrigger(ProducerDocumentsNotCreatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<ProducerDocumentsNotCreatedEvent>(message, token);
        }

        [FunctionName("ProducerDocumentsNotValidatedEvent")]
        public async Task ProducerDocumentsNotValidatedEventAsync([ServiceBusTrigger(ProducerDocumentsNotValidatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<ProducerDocumentsNotValidatedEvent>(message, token);
        }

        [FunctionName("ProducerDocumentsNotReviewedEvent")]
        public async Task ProducerDocumentsNotReviewedEventAsync([ServiceBusTrigger(ProducerDocumentsNotReviewedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<ProducerDocumentsNotReviewedEvent>(message, token);
        }

        [FunctionName("ProducerNotConfiguredEvent")]
        public async Task ProducerNotConfiguredEventAsync([ServiceBusTrigger(ProducerNotConfiguredEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<ProducerNotConfiguredEvent>(message, token);
        }
    }
}
