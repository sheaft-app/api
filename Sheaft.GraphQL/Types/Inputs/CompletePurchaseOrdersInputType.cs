using HotChocolate.Types;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CompletePurchaseOrdersInputType : SheaftInputType<CompletePurchaseOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CompletePurchaseOrdersCommand> descriptor)
        {
            descriptor.Name("CompletePurchaseOrdersInput");
            descriptor.Field(c => c.PurchaseOrderIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}