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
    public class PayinFunctions
    {
        private readonly IMediator _mediatr;

        public PayinFunctions(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("SetPayinFailedCommand")]
        public async Task SetPayinFailedCommandAsync([ServiceBusTrigger(SetPayinFailedCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetPayinFailedCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetPayinRefundFailedCommand")]
        public async Task SetPayinRefundFailedCommandAsync([ServiceBusTrigger(SetPayinRefundFailedCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetPayinRefundFailedCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetPayinRefundSucceededCommand")]
        public async Task SetPayinRefundSucceededCommandAsync([ServiceBusTrigger(SetPayinRefundSucceededCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetPayinRefundSucceededCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetPayinSucceededCommand")]
        public async Task SetPayinSucceededCommandAsync([ServiceBusTrigger(SetPayinSucceededCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetPayinSucceededCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }
    }
}
