using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Mediator;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Behaviours
{
    internal class PerformanceBehaviour<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
        where TResponse: IResult
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

        public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken token,
            MediatR.RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();

            if (_timer.ElapsedMilliseconds <= 1000)
                return response;

            var requestName = typeof(TRequest).Name;
            _logger.LogWarning(
                "Command {Name} took more than {ElapsedMilliseconds} milliseconds to complete for user {UserId}.",
                requestName, _timer.ElapsedMilliseconds, request.RequestUser.Id);

            return response;
        }
    }
}