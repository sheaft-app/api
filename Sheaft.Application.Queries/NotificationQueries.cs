using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;
using Microsoft.Extensions.Options;
using Sheaft.Options;

namespace Sheaft.Application.Queries
{
    public class NotificationQueries : INotificationQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly RoleOptions _roleOptions;

        public NotificationQueries(
            IAppDbContext context, 
            AutoMapper.IConfigurationProvider configurationProvider, 
            IOptionsSnapshot<RoleOptions> roleOptions)
        {
            _roleOptions = roleOptions.Value;
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<NotificationDto> GetNotification(Guid id, RequestUser currentUser)
        {
            try
            {
                return _context.Notifications
                    .Get(c => c.Id == id && c.User.Id == currentUser.Id)
                    .ProjectTo<NotificationDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<NotificationDto>().AsQueryable();
            }
        }

        public IQueryable<NotificationDto> GetNotifications(RequestUser currentUser)
        {
            try
            {
                return _context.Notifications
                    .Get(c => c.User.Id == currentUser.Id)
                    .ProjectTo<NotificationDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<NotificationDto>().AsQueryable();
            }
        }
    }
}