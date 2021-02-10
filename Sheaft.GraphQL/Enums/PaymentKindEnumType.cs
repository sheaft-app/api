using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class PaymentKindEnumType : EnumType<PaymentKind>
    {
        protected override void Configure(IEnumTypeDescriptor<PaymentKind> descriptor)
        {
            descriptor.Value(PaymentKind.Card).Name("CARD");
            descriptor.Value(PaymentKind.BankAccount).Name("BANK_ACCOUNT");
        }
    }
}
