using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeletePurchaseOrdersInputType : SheaftInputType<DeletePurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeletePurchaseOrdersCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeletePurchaseOrdersInput");
            
            descriptor
                .Field(c => c.PurchaseOrderIds)
                .Name("ids")
                .ID(nameof(PurchaseOrder));
        }
    }
}