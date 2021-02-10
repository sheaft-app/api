using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Notification.Queries
{
    public class NotificationQueries : INotificationQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public NotificationQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<NotificationDto> GetNotification(Guid id, RequestUser currentUser)
        {
            return _context.Notifications
                .Get(c => c.Id == id && c.User.Id == currentUser.Id)
                .ProjectTo<NotificationDto>(_configurationProvider);
        }

        public async Task<int> GetUnreadNotificationsCount(RequestUser currentUser, CancellationToken token)
        {
            return await _context.Notifications
                .CountAsync(c => c.Unread && c.User.Id == currentUser.Id, token);
        }

        public IQueryable<NotificationDto> GetNotifications(RequestUser currentUser)
        {
            return _context.Notifications
                .Get(c => c.User.Id == currentUser.Id)
                .ProjectTo<NotificationDto>(_configurationProvider);
        }
    }
}