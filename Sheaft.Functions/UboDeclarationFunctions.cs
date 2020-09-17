using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Logging;

namespace Sheaft.Functions
{
    public class UboDeclarationFunctions
    {
        private readonly IMediator _mediatr;

        public UboDeclarationFunctions(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("SetUboDeclarationIncompleteCommand")]
        public async Task SetUboDeclarationIncompleteCommandAsync([ServiceBusTrigger(SetUboDeclarationIncompleteCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetUboDeclarationIncompleteCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetUboDeclarationRefusedCommand")]
        public async Task SetUboDeclarationRefusedCommandAsync([ServiceBusTrigger(SetUboDeclarationRefusedCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetUboDeclarationRefusedCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetUboDeclarationValidatedCommand")]
        public async Task SetUboDeclarationValidatedCommandAsync([ServiceBusTrigger(SetUboDeclarationValidatedCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetUboDeclarationRefusedCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetUboDeclarationValidationCommand")]
        public async Task SetUboDeclarationValidationCommandAsync([ServiceBusTrigger(SetUboDeclarationValidationCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetUboDeclarationRefusedCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }
    }
}
