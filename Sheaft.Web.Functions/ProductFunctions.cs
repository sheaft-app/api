using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Interop;

namespace Sheaft.Functions
{
    public class ProductFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public ProductFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("ImportProductsCommand")]
        public async Task ImportProductsCommandAsync([ServiceBusTrigger(ImportProductsCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<ImportProductsCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("ProductsImportSucceededEvent")]
        public async Task ProductsImportSucceededEventAsync([ServiceBusTrigger(ProductImportSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<ProductImportSucceededEvent>(message, token);
        }

        [FunctionName("ProductsImportFailedEvent")]
        public async Task ProductsImportFailedEventAsync([ServiceBusTrigger(ProductImportFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<ProductImportFailedEvent>(message, token);
        }

        [FunctionName("ProductImportProcessingEvent")]
        public async Task ProductImportProcessingEventAsync([ServiceBusTrigger(ProductImportProcessingEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            await _mediatr.Process<ProductImportProcessingEvent>(message, token);
        }
    }
}
