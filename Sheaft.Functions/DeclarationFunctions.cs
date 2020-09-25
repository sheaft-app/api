using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Functions
{
    public class DeclarationFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public DeclarationFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("RefreshDeclarationStatusCommand")]
        public async Task RefreshDeclarationStatusCommandAsync([ServiceBusTrigger(RefreshDeclarationStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<RefreshDeclarationStatusCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("DeclarationIncompleteEvent")]
        public async Task DeclarationIncompleteEventAsync([ServiceBusTrigger(DeclarationIncompleteEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<DeclarationIncompleteEvent>(message, token);
        }

        [FunctionName("DeclarationRefusedEvent")]
        public async Task DeclarationRefusedEventAsync([ServiceBusTrigger(DeclarationRefusedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<DeclarationRefusedEvent>(message, token);
        }

        [FunctionName("DeclarationValidatedEvent")]
        public async Task DeclarationValidatedEventAsync([ServiceBusTrigger(DeclarationValidatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<DeclarationValidatedEvent>(message, token);
        }
    }
}
