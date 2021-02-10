using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class NotificationType : SheaftOutputType<NotificationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<NotificationDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Unread);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Method);
            descriptor.Field(c => c.Content);
        }
    }
}
