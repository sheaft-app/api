using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Logging;

namespace Sheaft.Functions
{
    public class DeclarationFunctions
    {
        private readonly IMediator _mediatr;

        public DeclarationFunctions(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("SetDeclarationStatusCommand")]
        public async Task SetDeclarationStatusCommandAsync([ServiceBusTrigger(SetDeclarationStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetDeclarationStatusCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("DeclarationIncompleteEvent")]
        public async Task DeclarationIncompleteEventAsync([ServiceBusTrigger(DeclarationIncompleteEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<DeclarationIncompleteEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.DeclarationId.ToString("N"));
        }

        [FunctionName("DeclarationRefusedEvent")]
        public async Task DeclarationRefusedEventAsync([ServiceBusTrigger(DeclarationRefusedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<DeclarationRefusedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.DeclarationId.ToString("N"));
        }

        [FunctionName("DeclarationValidatedEvent")]
        public async Task DeclarationValidatedEventAsync([ServiceBusTrigger(DeclarationValidatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<DeclarationValidatedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.DeclarationId.ToString("N"));
        }
    }
}
