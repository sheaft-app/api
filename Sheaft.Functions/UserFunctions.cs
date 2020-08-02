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
    public class UserFunctions
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediatr;

        public UserFunctions(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediatr = mediator;
        }

        [FunctionName("RemoveUserDataCommand")]
        public async Task RemoveUserDataCommandAsync([QueueTrigger(RemoveUserDataCommand.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<RemoveUserDataCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }
    }
}
