using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Commands;
using Sheaft.Core.Security;

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
        public async Task UpdateZoneProgressCommandAsync([TimerTrigger("*/60 * * * *", RunOnStartup = true)] TimerInfo info, ILogger logger, CancellationToken token)
        {
            var results = await _mediatr.Send(new UpdateZoneProgressCommand(new RequestUser("zone-functions", Guid.NewGuid().ToString("N"))), token);
            if (!results.Success)
            {
                throw results.Exception;
            }

            logger.LogInformation(nameof(ZoneFunctions.UpdateZoneProgressCommandAsync), "successfully executed");
        }
    }
}
