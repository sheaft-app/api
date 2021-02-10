using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class DeclarationStatusEnumType : EnumType<DeclarationStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<DeclarationStatus> descriptor)
        {
            descriptor.Value(DeclarationStatus.Created).Name("CREATED");
            descriptor.Value(DeclarationStatus.Incomplete).Name("INCOMPLETE");
            descriptor.Value(DeclarationStatus.Refused).Name("REFUSED");
            descriptor.Value(DeclarationStatus.Validated).Name("VALIDATED");
            descriptor.Value(DeclarationStatus.ValidationAsked).Name("VALIDATION_ASKED");
        }
    }
}
