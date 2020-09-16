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
using Sheaft.Core.Security;
using Sheaft.Logging;

namespace Sheaft.Functions
{
    public class HookFunctions
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediatr;

        public HookFunctions(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediatr = mediator;
        }

        [FunctionName("NewPspHookEvent")]
        public async Task NewPspHookEventAsync([ServiceBusTrigger(NewPspHookEvent.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<NewPspHookEvent>(message);
            await _mediatr.Publish(command, token);
        }
    }
}
