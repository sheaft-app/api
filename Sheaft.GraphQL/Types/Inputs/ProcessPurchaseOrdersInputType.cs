using HotChocolate.Types;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ProcessPurchaseOrdersInputType : SheaftInputType<ProcessPurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProcessPurchaseOrdersCommand> descriptor)
        {
            descriptor.Name("ProcessPurchaseOrdersInput");
            descriptor.Field(c => c.PurchaseOrderIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}