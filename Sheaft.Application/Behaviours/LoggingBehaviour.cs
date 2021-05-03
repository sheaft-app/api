using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Domain;

namespace Sheaft.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ITrackedUser
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation("Processing request: {Name} for {@UserId}", requestName, request.RequestUser.Name);
            
            using (var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = request.RequestUser.RequestId,
                ["UserIdentifier"] = request.RequestUser.Id.ToString("N"),
                ["UserEmail"] = request.RequestUser.Email,
                ["UserName"] = request.RequestUser.Name,
                ["Roles"] = string.Join(';', request.RequestUser.Roles),
                ["IsAuthenticated"] = request.RequestUser.IsAuthenticated().ToString(),
                ["Command"] = requestName,
                ["Data"] = JsonConvert.SerializeObject(request),
            }))
            {
                return await next();
            }
        }
    }
}