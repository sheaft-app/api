using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core;

namespace CleanArchitecture.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : ITrackedUser
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation("Processing request: {Name} for {@UserId} {@Request}",
            requestName, request.RequestUser.Name, request);

            await Task.CompletedTask;
        }
    }
}