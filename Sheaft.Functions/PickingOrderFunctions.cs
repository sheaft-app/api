using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Functions
{
    public class PickingOrderFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public PickingOrderFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("ExportPickingOrderCommand")]
        public async Task ExportPickingOrderCommandAsync([ServiceBusTrigger(ExportPickingOrderCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<ExportPickingOrderCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("PickingOrderExportSucceededEvent")]
        public async Task PickingOrderExportSucceededEventAsync([ServiceBusTrigger(PickingOrderExportSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PickingOrderExportSucceededEvent>(message, token);
        }

        [FunctionName("PickingOrderExportFailedEvent")]
        public async Task PickingOrderExportFailedEventAsync([ServiceBusTrigger(PickingOrderExportFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PickingOrderExportFailedEvent>(message, token);
        }

        [FunctionName("PickingOrderExportProcessingEvent")]
        public async Task PickingOrderExportProcessingEventAsync([ServiceBusTrigger(PickingOrderExportProcessingEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PickingOrderExportProcessingEvent>(message, token);
        }
    }
}
