using HotChocolate.Types;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CancelPurchaseOrdersInputType : SheaftInputType<CancelPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CancelPurchaseOrdersCommand> descriptor)
        {
            descriptor.Name("CancelPurchaseOrdersInput");
            descriptor.Field(c => c.PurchaseOrderIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.Reason);
        }
    }
}