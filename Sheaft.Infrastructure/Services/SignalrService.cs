using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Services;

internal class SignalrService : ISignalrService
{
    private readonly IHubContext<SheaftHub> _hubContext;
    private readonly ILogger<SignalrService> _logger;

    public SignalrService(
        IHubContext<SheaftHub> hubContext,
        ILogger<SignalrService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task<Result> SendNotificationTo<T>(string userIdentifier, string eventName, T content,
        CancellationToken token)
    {
        try
        {
            await _hubContext.Clients.User(userIdentifier).SendAsync("event",
                new {Method = eventName, UserId = userIdentifier, Content = content}, token);
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e.Message, e);
            return Result.Failure(ErrorKind.BadRequest, "signalr.error", e.Message);
        }
    }
}