using HotChocolate.Types;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class WithdrawnPurchaseOrdersInputType : SheaftInputType<WithdrawnPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<WithdrawnPurchaseOrdersCommand> descriptor)
        {
            descriptor.Name("WithdrawnPurchaseOrdersInput");
            descriptor.Field(c => c.PurchaseOrderIds)
                .Name("Ids")
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.Reason);
        }
    }
}