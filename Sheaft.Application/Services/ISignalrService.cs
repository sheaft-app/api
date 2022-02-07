using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Services
{
    public interface ISignalrService
    {
        Task<Result> SendNotificationToGroupAsync<T>(Guid groupId, string eventName, T content, CancellationToken token);
        Task<Result> SendNotificationToUserAsync<T>(Guid userId, string eventName, T content, CancellationToken token);
    }
}