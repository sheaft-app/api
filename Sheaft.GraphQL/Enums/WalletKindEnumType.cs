using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class WalletKindEnumType : EnumType<WalletKind>
    {
        protected override void Configure(IEnumTypeDescriptor<WalletKind> descriptor)
        {
            descriptor.Value(WalletKind.Donations).Name("DONATIONS");
            descriptor.Value(WalletKind.Returnables).Name("RETURNABLE");
            descriptor.Value(WalletKind.Documents).Name("DOCUMENTS");
            descriptor.Value(WalletKind.Payments).Name("PAYMENTS");
            descriptor.Value(WalletKind.Refunds).Name("REFUNDS");
        }
    }
}
