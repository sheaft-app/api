using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Functions
{
    public class PurchaseOrderFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public PurchaseOrderFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("PurchaseOrderAcceptedEvent")]
        public async Task PurchaseOrderAcceptedEventAsync([ServiceBusTrigger(PurchaseOrderAcceptedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PurchaseOrderAcceptedEvent>(message, token);
        }

        [FunctionName("PurchaseOrderCancelledBySenderEvent")]
        public async Task PurchaseOrderCancelledBySenderEventAsync([ServiceBusTrigger(PurchaseOrderCancelledBySenderEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PurchaseOrderCancelledBySenderEvent>(message, token);
        }

        [FunctionName("PurchaseOrderCancelledByVendorEvent")]
        public async Task PurchaseOrderCancelledByVendorEventAsync([ServiceBusTrigger(PurchaseOrderCancelledByVendorEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PurchaseOrderCancelledByVendorEvent>(message, token);
        }

        [FunctionName("PurchaseOrderCompletedEvent")]
        public async Task PurchaseOrderCompletedEventAsync([ServiceBusTrigger(PurchaseOrderCompletedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PurchaseOrderCompletedEvent>(message, token);
        }

        [FunctionName("PurchaseOrderProcessingEvent")]
        public async Task PurchaseOrderProcessingEventAsync([ServiceBusTrigger(PurchaseOrderProcessingEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PurchaseOrderProcessingEvent>(message, token);
        }

        [FunctionName("PurchaseOrderCreatedEvent")]
        public async Task PurchaseOrderCreatedEventAsync([ServiceBusTrigger(PurchaseOrderCreatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PurchaseOrderCreatedEvent>(message, token);
        }

        [FunctionName("PurchaseOrderRefusedEvent")]
        public async Task PurchaseOrderRefusedEventAsync([ServiceBusTrigger(PurchaseOrderRefusedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<PurchaseOrderRefusedEvent>(message, token);
        }
    }
}
