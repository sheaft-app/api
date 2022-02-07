using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Services;
using Sheaft.Domain.Common;
using Sheaft.Infrastructure.Hubs;

namespace Sheaft.Infrastructure.Services
{
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

        public async Task<Result> SendNotificationToGroupAsync<T>(Guid groupId, string eventName, T content, CancellationToken token)
        {
            try
            {
                await _hubContext.Clients.Group(groupId.ToString("N")).SendAsync("event", new { Method = eventName, GroupName = groupId.ToString("N"), Content = content }, token);
                return Result.Success();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message, e);
                return Result.Error(e);
            }
        }

        public async Task<Result> SendNotificationToUserAsync<T>(Guid userId, string eventName, T content, CancellationToken token)
        {
            try
            {
                await _hubContext.Clients.User(userId.ToString("N")).SendAsync("event", new { Method = eventName, UserId = userId.ToString("N"), Content = content }, token);
                return Result.Success();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message, e);
                return Result.Error(e);
            }
        }
    }
}
