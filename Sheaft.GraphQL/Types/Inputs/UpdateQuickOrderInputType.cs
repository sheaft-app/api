using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateQuickOrderInputType : SheaftInputType<UpdateQuickOrderDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateQuickOrderDto> descriptor)
        {
            descriptor.Name("UpdateQuickOrderInput");
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
            
            descriptor.Field(c => c.Products)
                .Type<ListType<ResourceIdQuantityInputType>>();
        }
    }
}
