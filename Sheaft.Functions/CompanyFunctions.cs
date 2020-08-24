using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Logging;

namespace Sheaft.Functions
{
    public class CompanyFunctions
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediatr;

        public CompanyFunctions(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediatr = mediator;
        }

        [FunctionName("RemoveCompanyDataCommand")]
        public async Task RemoveCompanyDataCommandAsync([ServiceBusTrigger(RemoveCompanyDataCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")]string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<RemoveCompanyDataCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }
    }
}
