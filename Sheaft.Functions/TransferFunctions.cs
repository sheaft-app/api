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
    public class TransferFunctions
    {
        private readonly IMediator _mediatr;

        public TransferFunctions(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("SetTransferFailedCommand")]
        public async Task SetTransferFailedCommandAsync([ServiceBusTrigger(SetTransferFailedCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetTransferFailedCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetTransferRefundFailedCommand")]
        public async Task SetTransferRefundFailedCommandAsync([ServiceBusTrigger(SetTransferRefundFailedCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetTransferRefundFailedCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetTransferRefundSucceededCommand")]
        public async Task SetTransferRefundSucceededCommandAsync([ServiceBusTrigger(SetTransferRefundSucceededCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetTransferRefundSucceededCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("SetTransferSucceededCommand")]
        public async Task SetTransferSucceededCommandAsync([ServiceBusTrigger(SetTransferSucceededCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetTransferSucceededCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }
    }
}
