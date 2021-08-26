using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Notifications
{
    [ExtendObjectType(Name = "Query")]
    public class NotificationQueries : SheaftQuery
    {
        public NotificationQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("notification")]
        [GraphQLType(typeof(NotificationType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UseSingleOrDefault]
        public IQueryable<Notification> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Notifications
                .Where(c => c.Id == id && c.UserId == CurrentUser.Id);
        }

        [GraphQLName("notifications")]
        [GraphQLType(typeof(ListType<NotificationType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Notification> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.Notifications
                .Where(c => c.UserId == CurrentUser.Id);
        }

        [GraphQLName("unreadNotificationsCount")]
        [GraphQLType(typeof(IntType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        public async Task<int> UnreadCount([ScopedService] QueryDbContext context, CancellationToken token)
        {
            SetLogTransaction();
            return await context.Notifications
                .CountAsync(c => c.Unread && c.UserId == CurrentUser.Id, token);
        }
    }
}