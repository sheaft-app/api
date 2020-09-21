using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class WalletKindEnumType : EnumType<WalletKind>
    {
        protected override void Configure(IEnumTypeDescriptor<WalletKind> descriptor)
        {
            descriptor.Value(WalletKind.Payments).Name("PAYMENTS");
            descriptor.Value(WalletKind.Returnables).Name("RETURNABLE");
        }
    }
}
