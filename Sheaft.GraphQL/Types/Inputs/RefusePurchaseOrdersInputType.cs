using HotChocolate.Types;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RefusePurchaseOrdersInputType : SheaftInputType<RefusePurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RefusePurchaseOrdersCommand> descriptor)
        {
            descriptor.Name("RefusePurchaseOrdersInput");
            descriptor.Field(c => c.PurchaseOrderIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.Reason);
        }
    }
}