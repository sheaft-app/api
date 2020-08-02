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
    public class UserPointsFunctions
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediatr;

        public UserPointsFunctions(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediatr = mediator;
        }

        [FunctionName("CreateUserPointsCommand")]
        public async Task CreateUserPointsCommandAsync([QueueTrigger(CreateUserPointsCommand.QUEUE_NAME, Connection = "AzureWebJobsStorage")]string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<CreateUserPointsCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }
    }
}
