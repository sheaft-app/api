using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Logging;

namespace Sheaft.Functions
{
    public class PickingOrderFunctions
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediatr;

        public PickingOrderFunctions(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediatr = mediator;
        }

        [FunctionName("ExportPickingOrderCommand")]
        public async Task ExportPickingOrderCommandAsync([QueueTrigger(ExportPickingOrderCommand.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<ExportPickingOrderCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("PickingOrderExportSucceededEvent")]
        public async Task PickingOrderExportSucceededEventAsync([QueueTrigger(PickingOrderExportSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PickingOrderExportSucceededEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }

        [FunctionName("PickingOrderExportFailedEvent")]
        public async Task PickingOrderExportFailedEventAsync([QueueTrigger(PickingOrderExportFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PickingOrderExportFailedEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }

        [FunctionName("PickingOrderExportProcessingEvent")]
        public async Task PickingOrderExportProcessingEventAsync([QueueTrigger(PickingOrderExportProcessingEvent.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PickingOrderExportProcessingEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }
    }
}
