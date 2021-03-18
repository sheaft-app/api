using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateQuickOrderInputType : SheaftInputType<CreateQuickOrderDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateQuickOrderDto> descriptor)
        {
            descriptor.Name("CreateQuickOrderInput");
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Products);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
