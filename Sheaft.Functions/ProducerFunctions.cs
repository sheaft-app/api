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
    public class ProducerFunctions
    {
        private readonly IMediator _mediatr;

        public ProducerFunctions(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("ProducerDocumentsNotCreatedEvent")]
        public async Task ProducerDocumentsNotCreatedEventAsync([ServiceBusTrigger(ProducerDocumentsNotCreatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<ProducerDocumentsNotCreatedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.UserId.ToString("N"));
        }

        [FunctionName("ProducerDocumentsNotValidatedEvent")]
        public async Task ProducerDocumentsNotValidatedEventAsync([ServiceBusTrigger(ProducerDocumentsNotValidatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<ProducerDocumentsNotValidatedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.UserId.ToString("N"));
        }

        [FunctionName("ProducerDocumentsNotReviewedEvent")]
        public async Task ProducerDocumentsNotReviewedEventAsync([ServiceBusTrigger(ProducerDocumentsNotReviewedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<ProducerDocumentsNotReviewedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.UserId.ToString("N"));
        }

        [FunctionName("ProducerNotConfiguredEvent")]
        public async Task ProducerNotConfiguredEventAsync([ServiceBusTrigger(ProducerNotConfiguredEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<ProducerNotConfiguredEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.UserId.ToString("N"));
        }
    }
}
