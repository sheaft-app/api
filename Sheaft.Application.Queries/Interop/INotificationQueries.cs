using Sheaft.Application.Models;
using Sheaft.Core;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Queries
{
    public interface INotificationQueries
    {
        IQueryable<NotificationDto> GetNotification(Guid id, RequestUser currentUser);
        IQueryable<NotificationDto> GetNotifications(RequestUser currentUser);
        Task<int> GetUnreadNotificationsCount(RequestUser currentUser, CancellationToken token);
    }
}