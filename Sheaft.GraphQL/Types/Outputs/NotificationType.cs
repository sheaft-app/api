using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Notifications;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class NotificationType : SheaftOutputType<Notification>
    {
        protected override void Configure(IObjectTypeDescriptor<Notification> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<NotificationsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
                
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");
                
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.Unread)
                .Name("unread");
                
            descriptor
                .Field(c => c.Method)
                .Name("method");
                
            descriptor
                .Field(c => c.Content)
                .Name("content");
                
            descriptor
                .Field(c => c.User)
                .Name("user")
                .ResolveWith<NotificationResolvers>(c => c.GetUser(default, default, default))
                .Type<NonNullType<UserType>>();
        }

        private class NotificationResolvers
        {
            public Task<User> GetUser(Notification notification, UsersByIdBatchDataLoader usersDataLoader, CancellationToken token)
            {
                return usersDataLoader.LoadAsync(notification.UserId, token);
            }
        }
    }
}
