using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ShipPurchaseOrdersInputType : SheaftInputType<ShipPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ShipPurchaseOrdersCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ShipPurchaseOrdersInput");

            descriptor
                .Field(c => c.PurchaseOrderIds)
                .Name("ids")
                .ID(nameof(PurchaseOrder));
        }
    }
}