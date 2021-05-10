using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Notification.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class MarkUserNotificationAsReadInputType : SheaftInputType<MarkUserNotificationAsReadCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<MarkUserNotificationAsReadCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("MarkUserNotificationAsReadInput");
            
            descriptor
                .Field(c => c.NotificationId)
                .Name("id")
                .ID(nameof(Notification));
        }
    }
}