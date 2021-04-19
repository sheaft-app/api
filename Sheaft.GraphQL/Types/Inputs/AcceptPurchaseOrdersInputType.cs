using HotChocolate.Types;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AcceptPurchaseOrdersInputType : SheaftInputType<AcceptPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AcceptPurchaseOrdersCommand> descriptor)
        {
            descriptor.Name("AcceptPurchaseOrdersInput");
            descriptor.Field(c => c.PurchaseOrderIds)
                .Name("Ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}