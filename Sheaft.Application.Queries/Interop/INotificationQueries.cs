using Sheaft.Models.Dto;
using Sheaft.Core;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface INotificationQueries
    {
        IQueryable<NotificationDto> GetNotification(Guid id, RequestUser currentUser);
        IQueryable<NotificationDto> GetNotifications(RequestUser currentUser);
    }
}