using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AcceptPurchaseOrdersInputType : SheaftInputType<AcceptPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AcceptPurchaseOrdersCommand> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("AcceptPurchaseOrdersInput");
            
            descriptor
                .Field(c => c.PurchaseOrderIds)
                .Name("ids")
                .ID(nameof(PurchaseOrder));
        }
    }
}