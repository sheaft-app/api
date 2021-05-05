using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CancelPurchaseOrdersInputType : SheaftInputType<CancelPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CancelPurchaseOrdersCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CancelPurchaseOrdersInput");
            
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