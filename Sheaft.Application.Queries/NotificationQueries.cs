using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Interop;
using Sheaft.Core.Security;
using Sheaft.Core.Extensions;
using Sheaft.Domain.Models;
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

        public IQueryable<NotificationDto> GetNotification(Guid id, IRequestUser currentUser)
        {
            try
            {
                if (currentUser.IsInRole(RoleNames.CONSUMER))
                    return _context.Notifications
                        .Get(c => c.Id == id && c.User.Id == currentUser.Id)
                        .ProjectTo<NotificationDto>(_configurationProvider);

                return _context.Notifications
                        .Get(c => c.Id == id && (c.User != null && c.User.Id == currentUser.Id || c.Group != null && c.Group.Id == currentUser.CompanyId))
                        .ProjectTo<NotificationDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<NotificationDto>().AsQueryable();
            }
        }

        public IQueryable<NotificationDto> GetNotifications(IRequestUser currentUser)
        {
            try
            {
                if (currentUser.IsInRole(RoleNames.CONSUMER))
                    return _context.Notifications
                        .Get(c => c.User != null && c.User.Id == currentUser.Id)
                        .ProjectTo<NotificationDto>(_configurationProvider);

                return _context.Notifications
                        .Get(c => (c.User != null && c.User.Id == currentUser.Id || c.Group != null && c.Group.Id == currentUser.CompanyId))
                        .ProjectTo<NotificationDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<NotificationDto>().AsQueryable();
            }
        }
        private static IQueryable<NotificationDto> GetAsDto(IQueryable<Notification> query)
        {
            return query
                .Select(c => new NotificationDto
                {
                    Id = c.Id,
                    CreatedOn = c.CreatedOn,
                    Content = c.Content,
                    Kind = c.Kind,
                    Method = c.Method,
                    Unread = c.Unread,
                    UpdatedOn = c.UpdatedOn
                });
        }
    }
}