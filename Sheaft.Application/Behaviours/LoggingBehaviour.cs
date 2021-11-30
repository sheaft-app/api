﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Domain;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Behaviours
{
    internal class LoggingBehaviour<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
        where TRequest : ITrackedUser
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken token,
            MediatR.RequestHandlerDelegate<TResponse> next)
        {
            if (request.RequestUser == null)
                throw new Exception("The requestUser must be assigned for the command.");
            
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