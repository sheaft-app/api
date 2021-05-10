using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeliverPurchaseOrdersInputType : SheaftInputType<DeliverPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeliverPurchaseOrdersCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeliverPurchaseOrdersInput");
            
            descriptor
                .Field(c => c.PurchaseOrderIds)
                .Name("ids")
                .ID(nameof(PurchaseOrder));
        }
    }
}