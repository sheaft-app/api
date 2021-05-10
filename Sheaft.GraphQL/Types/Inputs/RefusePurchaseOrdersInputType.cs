using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RefusePurchaseOrdersInputType : SheaftInputType<RefusePurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RefusePurchaseOrdersCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RefusePurchaseOrdersInput");

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