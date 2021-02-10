using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class UpdateQuickOrderInputType : SheaftInputType<UpdateQuickOrderInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateQuickOrderInput> descriptor)
        {
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
