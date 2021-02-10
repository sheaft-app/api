using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class AddressKindEnumType : EnumType<AddressKind>
    {
        protected override void Configure(IEnumTypeDescriptor<AddressKind> descriptor)
        {
            descriptor.Value(AddressKind.Billing).Name("BILLING");
            descriptor.Value(AddressKind.Contact).Name("CONTACT");
            descriptor.Value(AddressKind.Legals).Name("LEGALS");
            descriptor.Value(AddressKind.Shipping).Name("SHIPPING");
        }
    }
}
