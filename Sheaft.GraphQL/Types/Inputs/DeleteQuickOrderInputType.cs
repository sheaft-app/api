using HotChocolate.Types;
using Sheaft.Mediatr.QuickOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteQuickOrdersInputType : SheaftInputType<DeleteQuickOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteQuickOrdersCommand> descriptor)
        {
            descriptor.Name("DeleteQuickOrdersInput");
            
            descriptor.Field(c => c.QuickOrderIds)
                .Name("Ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}