using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class PreAuthorizationStatusEnumType : EnumType<PreAuthorizationStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<PreAuthorizationStatus> descriptor)
        {
            descriptor.Value(PreAuthorizationStatus.Created).Name("CREATED");
            descriptor.Value(PreAuthorizationStatus.Failed).Name("FAILED");
            descriptor.Value(PreAuthorizationStatus.Succeeded).Name("SUCCEEDED");
        }
    }
}