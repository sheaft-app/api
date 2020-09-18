using System;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
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

        public IQueryable<NotificationDto> GetNotifications(RequestUser currentUser)
        {
            return _context.Notifications
                .Get(c => c.User.Id == currentUser.Id)
                .ProjectTo<NotificationDto>(_configurationProvider);
        }
    }
}