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
    public class ProductFunctions
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediatr;

        public ProductFunctions(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediatr = mediator;
        }

        [FunctionName("ImportProductsCommand")]
        public async Task ImportProductsCommandAsync([ServiceBusTrigger(ImportProductsCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<ImportProductsCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("ProductsImportSucceededEvent")]
        public async Task ProductsImportSucceededEventAsync([ServiceBusTrigger(ProductImportSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<ProductImportSucceededEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }

        [FunctionName("ProductsImportFailedEvent")]
        public async Task ProductsImportFailedEventAsync([ServiceBusTrigger(ProductImportFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<ProductImportFailedEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }

        [FunctionName("ProductImportProcessingEvent")]
        public async Task ProductImportProcessingEventAsync([ServiceBusTrigger(ProductImportProcessingEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<ProductImportProcessingEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }
    }
}
