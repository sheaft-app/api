using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class PointKindEnumType : EnumType<PointKind>
    {
        protected override void Configure(IEnumTypeDescriptor<PointKind> descriptor)
        {
            descriptor.Value(PointKind.BugBounty).Name("BUG_BOUNTY");
            descriptor.Value(PointKind.PurchaseOrder).Name("PURCHASE_ORDER");
            descriptor.Value(PointKind.RateProduct).Name("RATE_PRODUCT");
            descriptor.Value(PointKind.Sponsoring).Name("SPONSORING");
        }
    }
}
