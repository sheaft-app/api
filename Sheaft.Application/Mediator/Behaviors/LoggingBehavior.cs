using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Domain;

namespace Sheaft.Application;

internal class LoggingBehavior<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken token,
        MediatR.RequestHandlerDelegate<TResponse> next)
    {
        var requestName = typeof(TRequest).Name;

        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["Command"] = requestName,
            ["Data"] = JsonConvert.SerializeObject(request),
        });

        _logger.LogInformation("Processing request: {Name}", requestName);

        var result = await next();
        if (result.IsFailure)
            _logger.LogInformation("Failed to process request {Name} with error: {Error}", requestName,
                result.Error.Message);
        else
            _logger.LogInformation("Processed request {Name} successfully", requestName);

        return result;
    }
}