using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.QuickOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteQuickOrdersInputType : SheaftInputType<DeleteQuickOrdersCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteQuickOrdersCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteQuickOrdersInput");
            
            descriptor
                .Field(c => c.QuickOrderIds)
                .Name("ids")
                .ID(nameof(QuickOrder));
        }
    }
}