using HotChocolate.Types;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ShipPurchaseOrdersInputType : SheaftInputType<ShipPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ShipPurchaseOrdersCommand> descriptor)
        {
            descriptor.Name("ShipPurchaseOrdersInput");
            descriptor.Field(c => c.PurchaseOrderIds)
                .Name("Ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}