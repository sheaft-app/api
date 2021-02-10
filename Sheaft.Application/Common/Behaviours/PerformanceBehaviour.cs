using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core;

namespace CleanArchitecture.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ITrackedUser
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehaviour(
            ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            if (_timer.ElapsedMilliseconds <= 500)
                return response;

            var requestName = typeof(TRequest).Name;
            _logger.LogWarning(
                "Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
                requestName, _timer.ElapsedMilliseconds, request.RequestUser.Name, request);

            return response;
        }
    }
}