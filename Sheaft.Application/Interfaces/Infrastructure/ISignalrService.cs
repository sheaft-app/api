using System;
using System.Threading.Tasks;

namespace Sheaft.Application.Interfaces.Infrastructure
{
    public interface ISignalrService
    {
        Task SendNotificationToGroupAsync<T>(Guid groupId, string eventName, T content);
        Task SendNotificationToUserAsync<T>(Guid userId, string eventName, T content);
    }
}