using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class WithdrawnPurchaseOrdersInputType : SheaftInputType<WithdrawnPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<WithdrawnPurchaseOrdersCommand> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("WithdrawnPurchaseOrdersInput");
            
            descriptor
                .Field(c => c.PurchaseOrderIds)
                .Name("ids")
                .ID(nameof(PurchaseOrder));

            descriptor
                .Field(c => c.Reason)
                .Name("reason");
        }
    }
}