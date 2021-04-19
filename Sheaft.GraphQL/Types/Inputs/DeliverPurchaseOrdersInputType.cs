using HotChocolate.Types;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeliverPurchaseOrdersInputType : SheaftInputType<DeliverPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeliverPurchaseOrdersCommand> descriptor)
        {
            descriptor.Name("DeliverPurchaseOrdersInput");
            descriptor.Field(c => c.PurchaseOrderIds)
                .Name("Ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}