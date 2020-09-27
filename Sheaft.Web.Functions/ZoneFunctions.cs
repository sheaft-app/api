using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using Sheaft.Core;

namespace Sheaft.Functions
{
    public class ZoneFunctions
    {
        private readonly ISheaftMediatr _mediatr;

        public ZoneFunctions(ISheaftMediatr mediator)
        {
            _mediatr = mediator;
        }

        [FunctionName("UpdateZoneProgressCommand")]
        public async Task UpdateZoneProgressCommandAsync([TimerTrigger("0 0 */6 * * *", RunOnStartup = false)] TimerInfo info, CancellationToken token)
        {
            var results = await _mediatr.Process(new UpdateZoneProgressCommand(new RequestUser("zone-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("GenerateZonesFileCommand")]
        public async Task GenerateZonesFileCommandAsync([TimerTrigger("0 0 */2 * * *", RunOnStartup = false)] TimerInfo info, CancellationToken token)
        {
            var results = await _mediatr.Process(new GenerateZonesFileCommand(new RequestUser("zone-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("UpdateDepartmentCommand")]
        public async Task UpdateDepartmentCommandAsync([ServiceBusTrigger(UpdateDepartmentStatsCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<UpdateDepartmentStatsCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }

        [FunctionName("UpdateRegionCommand")]
        public async Task UpdateRegionCommandAsync([ServiceBusTrigger(UpdateRegionStatsCommand.QUEUE_NAME, Connection = "AzureWebJobsServiceBus")] string message, CancellationToken token)
        {
            var results = await _mediatr.Process<UpdateRegionStatsCommand, bool>(message, token);
            if (!results.Success)
                throw results.Exception;
        }
    }
}
