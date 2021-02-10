using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class ProfileKindEnumType : EnumType<ProfileKind>
    {
        protected override void Configure(IEnumTypeDescriptor<ProfileKind> descriptor)
        {
            descriptor.Value(ProfileKind.Producer).Name("PRODUCER");
            descriptor.Value(ProfileKind.Store).Name("STORE");
            descriptor.Value(ProfileKind.Consumer).Name("CONSUMER");
        }
    }
}
