using HotChocolate.Types;
using Sheaft.Mediatr.Notification.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class MarkUserNotificationAsReadInputType : SheaftInputType<MarkUserNotificationAsReadCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<MarkUserNotificationAsReadCommand> descriptor)
        {
            descriptor.Name("MarkUserNotificationAsReadInput");
            descriptor.Field(c => c.NotificationId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}