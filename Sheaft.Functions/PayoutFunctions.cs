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
    public class PayoutFunctions
    {
        private readonly IMediator _mediatr;

        public PayoutFunctions(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("SetPayoutStatusCommand")]
        public async Task SetPayoutStatusCommandAsync([ServiceBusTrigger(SetPayoutStatusCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<SetPayoutStatusCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("PayoutFailedEvent")]
        public async Task PayoutFailedEventAsync([ServiceBusTrigger(PayoutFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PayoutFailedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.TransactionId.ToString("N"));
        }

        [FunctionName("PayoutSucceededEvent")]
        public async Task PayoutSucceededEventAsync([ServiceBusTrigger(PayoutSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PayoutSucceededEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.TransactionId.ToString("N"));
        }
    }
}
