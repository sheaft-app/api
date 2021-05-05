using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CompletePurchaseOrdersInputType : SheaftInputType<CompletePurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CompletePurchaseOrdersCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CompletePurchaseOrdersInput");
            
            descriptor
                .Field(c => c.PurchaseOrderIds)
                .Name("ids")
                .ID(nameof(PurchaseOrder));
        }
    }
}