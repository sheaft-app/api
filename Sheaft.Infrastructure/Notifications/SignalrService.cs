using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Notifications;
using Sheaft.Infrastructure.Hubs;

namespace Sheaft.Infrastructure.Notifications
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

        public async Task SendNotificationToGroupAsync<T>(Guid groupId, string eventName, T content, CancellationToken token)
        {
            try
            {
                await _hubContext.Clients.Group(groupId.ToString("N")).SendAsync("event", new { Method = eventName, GroupName = groupId.ToString("N"), Content = content }, token); 
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }

        public async Task SendNotificationToUserAsync<T>(Guid userId, string eventName, T content, CancellationToken token)
        {
            try
            {
                await _hubContext.Clients.User(userId.ToString("N")).SendAsync("event", new { Method = eventName, UserId = userId.ToString("N"), Content = content }, token);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}
