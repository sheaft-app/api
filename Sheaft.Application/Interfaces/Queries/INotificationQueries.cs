using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface INotificationQueries
    {
        IQueryable<NotificationDto> GetNotification(Guid id, RequestUser currentUser);
        IQueryable<NotificationDto> GetNotifications(RequestUser currentUser);
        Task<int> GetUnreadNotificationsCount(RequestUser currentUser, CancellationToken token);
    }
}