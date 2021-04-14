using HotChocolate.Types;
using Sheaft.Domain.Enum;

namespace Sheaft.GraphQL.Enums
{
    public class PurchaseOrderStatusEnumType : EnumType<PurchaseOrderStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<PurchaseOrderStatus> descriptor)
        {
            descriptor.Value(PurchaseOrderStatus.Accepted).Name("ACCEPTED");
            descriptor.Value(PurchaseOrderStatus.Cancelled).Name("CANCELLED");
            descriptor.Value(PurchaseOrderStatus.Completed).Name("COMPLETED");
            descriptor.Value(PurchaseOrderStatus.Delivered).Name("DELIVERED");
            descriptor.Value(PurchaseOrderStatus.Processing).Name("PROCESSING");
            descriptor.Value(PurchaseOrderStatus.Refused).Name("REFUSED");
            descriptor.Value(PurchaseOrderStatus.Shipping).Name("SHIPPING");
            descriptor.Value(PurchaseOrderStatus.Waiting).Name("WAITING");
            descriptor.Value(PurchaseOrderStatus.Withdrawned).Name("WITHDRAWNED");
            descriptor.Value(PurchaseOrderStatus.Expired).Name("EXPIRED");
        }
    }
}
