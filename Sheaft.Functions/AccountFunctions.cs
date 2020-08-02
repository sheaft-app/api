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
    public class AccountFunctions
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediatr;

        public AccountFunctions(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediatr = mediator;
        }

        [FunctionName("ExportAccountDataCommand")]
        public async Task ExportAccountDataCommandAsync([QueueTrigger(ExportAccountDataCommand.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<ExportAccountDataCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("AccountExportDataSucceededEvent")]
        public async Task AccountExportDataSucceededEventAsync([QueueTrigger(AccountExportDataSucceededEvent.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<AccountExportDataSucceededEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }

        [FunctionName("AccountExportDataFailedEvent")]
        public async Task AccountExportDataFailedEventAsync([QueueTrigger(AccountExportDataFailedEvent.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<AccountExportDataFailedEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }

        [FunctionName("AccountExportDataProcessingEvent")]
        public async Task AccountExportDataProcessingEventAsync([QueueTrigger(AccountExportDataProcessingEvent.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var appEvent = JsonConvert.DeserializeObject<AccountExportDataProcessingEvent>(message);
            await _mediatr.Publish(appEvent, token);
        }
    }
}
