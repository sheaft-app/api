using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
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
