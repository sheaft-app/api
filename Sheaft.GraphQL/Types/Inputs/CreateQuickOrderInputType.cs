using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class CreateQuickOrderInputType : SheaftInputType<CreateQuickOrderInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateQuickOrderInput> descriptor)
        {
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Products);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
