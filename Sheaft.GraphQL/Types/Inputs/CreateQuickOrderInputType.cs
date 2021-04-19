using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.QuickOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateQuickOrderInputType : SheaftInputType<CreateQuickOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateQuickOrderCommand> descriptor)
        {
            descriptor.Name("CreateQuickOrderInput");
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Products);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
