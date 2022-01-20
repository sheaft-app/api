using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Notifications
{
    public interface ISignalrService
    {
        Task SendNotificationToGroupAsync<T>(Guid groupId, string eventName, T content, CancellationToken token);
        Task SendNotificationToUserAsync<T>(Guid userId, string eventName, T content, CancellationToken token);
    }
}