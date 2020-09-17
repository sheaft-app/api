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
    public class PayoutFunctions
    {
        private readonly IMediator _mediatr;

        public PayoutFunctions(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("SetPayoutFailedCommand")]
        public async Task SetPayoutFailedCommandAsync([ServiceBusTrigger(SetPayoutFailedCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetPayoutFailedCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetPayoutSucceededCommand")]
        public async Task SetPayoutSucceededCommandAsync([ServiceBusTrigger(SetPayoutSucceededCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetPayoutSucceededCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }
    }
}
