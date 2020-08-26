using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Core.Security;
using Sheaft.Logging;

namespace Sheaft.Functions
{
    public class ZoneFunctions
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediatr;

        public ZoneFunctions(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediatr = mediator;
        }

        [FunctionName("UpdateZoneProgressCommand")]
        public async Task UpdateZoneProgressCommandAsync([TimerTrigger("0 0 */6 * * *", RunOnStartup = true)] TimerInfo info, ILogger logger, CancellationToken token)
        {
            var results = await _mediatr.Send(new UpdateZoneProgressCommand(new RequestUser("zone-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;

            logger.LogInformation(nameof(ZoneFunctions.UpdateZoneProgressCommandAsync), "successfully executed");
        }

        [FunctionName("GenerateZonesFileCommand")]
        public async Task GenerateZonesFileCommandAsync([TimerTrigger("0 0 */2 * * *", RunOnStartup = false)] TimerInfo info, ILogger logger, CancellationToken token)
        {
            var results = await _mediatr.Send(new GenerateZonesFileCommand(new RequestUser("zone-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;

            logger.LogInformation(nameof(ZoneFunctions.GenerateZonesFileCommandAsync), "successfully executed");
        }

        [FunctionName("UpdateDepartmentCommand")]
        public async Task UpdateDepartmentCommandAsync([ServiceBusTrigger(UpdateDepartmentStatsCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<UpdateDepartmentStatsCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("UpdateRegionCommand")]
        public async Task UpdateRegionCommandAsync([ServiceBusTrigger(UpdateRegionStatsCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, ILogger logger, CancellationToken token)
        {
            var command = JsonConvert.DeserializeObject<UpdateRegionStatsCommand>(message);
            var results = await _mediatr.Send(command, token);
            logger.LogCommand(results);

            if (!results.Success)
                throw results.Exception;

            logger.LogInformation(nameof(ZoneFunctions.UpdateRegionCommandAsync), "successfully executed");
        }
    }
}
