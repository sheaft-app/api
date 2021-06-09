using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Notification.Commands;

namespace Sheaft.GraphQL.Notifications
{
    [ExtendObjectType(Name = "Mutation")]
    public class NotificationMutations : SheaftMutation
    {
        public NotificationMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("markNotificationsAsRead")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(DateTimeType))]
        public async Task<DateTimeOffset> MarkMyNotificationsAsReadAsync([Service] ISheaftMediatr mediatr, CancellationToken token)
        {
            var input = new MarkUserNotificationsAsReadCommand(CurrentUser) {ReadBefore = DateTimeOffset.UtcNow};
            await ExecuteAsync(mediatr, input, token);
            return input.ReadBefore;
        }

        [GraphQLName("markNotificationAsRead")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(NotificationType))]
        public async Task<Notification> MarkNotificationAsReadAsync(
            [GraphQLType(typeof(MarkUserNotificationAsReadInputType))] [GraphQLName("input")]
            MarkUserNotificationAsReadCommand input, [Service] ISheaftMediatr mediatr,
            NotificationsByIdBatchDataLoader notificationQueries, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await notificationQueries.LoadAsync(input.NotificationId, token);
        }
    }
}