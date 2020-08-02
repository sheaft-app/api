using Sheaft.Models.Dto;
using Sheaft.Interop;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface INotificationQueries
    {
        IQueryable<NotificationDto> GetNotification(Guid id, IRequestUser currentUser);
        IQueryable<NotificationDto> GetNotifications(IRequestUser currentUser);
    }
}