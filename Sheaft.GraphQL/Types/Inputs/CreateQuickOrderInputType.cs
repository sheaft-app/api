using HotChocolate.Types;
using Sheaft.Mediatr.QuickOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateQuickOrderInputType : SheaftInputType<CreateQuickOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateQuickOrderCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateQuickOrderInput");
            
            descriptor
                .Field(c => c.Description)
                .Name("description");
            
            descriptor
                .Field(c => c.Products)
                .Name("products");

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();
        }
    }
}
