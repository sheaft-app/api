using HotChocolate.Types;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeletePurchaseOrdersInputType : SheaftInputType<DeletePurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeletePurchaseOrdersCommand> descriptor)
        {
            descriptor.Name("DeletePurchaseOrdersInput");
            descriptor.Field(c => c.PurchaseOrderIds)
                .Name("Ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}