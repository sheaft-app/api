using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class NotificationKindEnumType : EnumType<NotificationKind>
    {
        protected override void Configure(IEnumTypeDescriptor<NotificationKind> descriptor)
        {
            descriptor.Value(NotificationKind.Business).Name("BUSINESS");
            descriptor.Value(NotificationKind.None).Name("NONE");
            descriptor.Value(NotificationKind.Technical).Name("TECHNICAL");
        }
    }
}
