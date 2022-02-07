using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Mediator;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Behaviours
{
    internal class LoggingBehaviour<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
        where TResponse: IResult
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken token,
            MediatR.RequestHandlerDelegate<TResponse> next)
        {
            if (request.RequestUser == null)
                throw new Exception("The requestUser must be assigned for the command.");
            
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation("Processing request: {Name} for {@UserId}", requestName, request.RequestUser.Id);
            
            using (var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["UserId"] = request.RequestUser.Id,
                ["Command"] = requestName,
                ["Data"] = JsonConvert.SerializeObject(request),
            }))
            {
                return await next();
            }
        }
    }
}