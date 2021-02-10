using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class DonationKindEnumType : EnumType<DonationKind>
    {
        protected override void Configure(IEnumTypeDescriptor<DonationKind> descriptor)
        {
            descriptor.Value(DonationKind.None).Name("NONE");
            descriptor.Value(DonationKind.Euro).Name("EURO");
            descriptor.Value(DonationKind.Rounded).Name("ROUNDED");
            descriptor.Value(DonationKind.Free).Name("FREE");
        }
    }
}
