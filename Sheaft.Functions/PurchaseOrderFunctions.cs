using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Core;
using Sheaft.Logging;

namespace Sheaft.Functions
{
    public class PurchaseOrderFunctions
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediatr;

        public PurchaseOrderFunctions(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediatr = mediator;
        }

        [FunctionName("CreatePurchaseOrderTransfersCommand")]
        public async Task CreatePurchaseOrderTransfersCommandAsync([TimerTrigger("0 * */1 * * *", RunOnStartup = false)] TimerInfo info, ILogger logger, CancellationToken token)
        {
            var results = await _mediatr.Send(new CreatePurchaseOrderTransfersCommand(new RequestUser("purchaseorders-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;

            logger.LogInformation(nameof(CreatePurchaseOrderTransfersCommand), "successfully executed");
        }

        [FunctionName("CreatePurchaseOrderTransferCommand")]
        public async Task CreatePurchaseOrderTransferCommandAsync([ServiceBusTrigger(CreatePurchaseOrderTransferCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<CreatePurchaseOrderTransferCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("PurchaseOrderAcceptedEvent")]
        public async Task PurchaseOrderAcceptedEventAsync([ServiceBusTrigger(PurchaseOrderAcceptedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PurchaseOrderAcceptedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.Id.ToString("N"));
        }

        [FunctionName("PurchaseOrderCancelledBySenderEvent")]
        public async Task PurchaseOrderCancelledBySenderEventAsync([ServiceBusTrigger(PurchaseOrderCancelledBySenderEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PurchaseOrderCancelledBySenderEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.Id.ToString("N"));
        }


        [FunctionName("PurchaseOrderCancelledByVendorEvent")]
        public async Task PurchaseOrderCancelledByVendorEventAsync([ServiceBusTrigger(PurchaseOrderCancelledByVendorEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PurchaseOrderCancelledByVendorEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.Id.ToString("N"));
        }

        [FunctionName("PurchaseOrderCompletedEvent")]
        public async Task PurchaseOrderCompletedEventAsync([ServiceBusTrigger(PurchaseOrderCompletedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PurchaseOrderCompletedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.Id.ToString("N"));
        }

        [FunctionName("PurchaseOrderProcessingEvent")]
        public async Task PurchaseOrderProcessingEventAsync([ServiceBusTrigger(PurchaseOrderProcessingEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PurchaseOrderProcessingEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.Id.ToString("N"));
        }

        [FunctionName("PurchaseOrderCreatedEvent")]
        public async Task PurchaseOrderCreatedEventAsync([ServiceBusTrigger(PurchaseOrderCreatedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PurchaseOrderCreatedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.Id.ToString("N"));
        }

        [FunctionName("PurchaseOrderRefusedEvent")]
        public async Task PurchaseOrderRefusedEventAsync([ServiceBusTrigger(PurchaseOrderRefusedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<PurchaseOrderRefusedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.Id.ToString("N"));
        }

        [FunctionName("CreatePurchaseOrderTransferFailedEvent")]
        public async Task CreatePurchaseOrderTransferFailedEventAsync([ServiceBusTrigger(CreatePurchaseOrderTransferFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<CreatePurchaseOrderTransferFailedEvent>(message);
            await _mediatr.Publish(appEvent, token);
            logger.LogInformation(appEvent.PurchaseOrderId.ToString("N"));
        }
    }
}
