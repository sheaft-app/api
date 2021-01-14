using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class TransactionKindEnumType : EnumType<TransactionKind>
    {
        protected override void Configure(IEnumTypeDescriptor<TransactionKind> descriptor)
        {
            descriptor.Value(TransactionKind.WebPayin).Name("WEB_PAYIN");
            descriptor.Value(TransactionKind.CardPayin).Name("CARD");
            descriptor.Value(TransactionKind.PreAuthorizedPayin).Name("PRE_AUTHORIZED_PAYIN");
            descriptor.Value(TransactionKind.Transfer).Name("TRANSFER");
            descriptor.Value(TransactionKind.Payout).Name("PAYOUT");
            descriptor.Value(TransactionKind.RefundPayin).Name("REFUND_PAYIN");
            descriptor.Value(TransactionKind.RefundPayout).Name("REFUND_PAYOUT");
            descriptor.Value(TransactionKind.RefundTransfer).Name("REFUND_TRANSFER");
            descriptor.Value(TransactionKind.Repudiation).Name("REPUDIATION");
            descriptor.Value(TransactionKind.Settlement).Name("SETTLEMENT");
        }
    }
}
