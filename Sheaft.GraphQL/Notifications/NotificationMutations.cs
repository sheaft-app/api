using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.Mediatr.Notification.Commands;

namespace Sheaft.GraphQL.Notifications
{
    [ExtendObjectType(Name = "Mutation")]
    public class NotificationMutations : SheaftMutation
    {
        public NotificationMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("markNotificationsAsRead")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(DateType))]
        public async Task<DateTimeOffset> MarkMyNotificationsAsReadAsync(CancellationToken token)
        {
            var input = new MarkUserNotificationsAsReadCommand(CurrentUser) {ReadBefore = DateTimeOffset.UtcNow};
            await ExecuteAsync(input, token);
            return input.ReadBefore;
        }

        [GraphQLName("markNotificationAsRead")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(NotificationType))]
        public async Task<Notification> MarkNotificationAsReadAsync(
            [GraphQLType(typeof(MarkUserNotificationAsReadInputType))] [GraphQLName("input")]
            MarkUserNotificationAsReadCommand input,
            NotificationsByIdBatchDataLoader notificationQueries, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await notificationQueries.LoadAsync(input.NotificationId, token);
        }
    }
}