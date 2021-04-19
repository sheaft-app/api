using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ResourceIdQuantityInputType : SheaftInputType<ResourceIdQuantityInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResourceIdQuantityInputDto> descriptor)
        {
            descriptor.Name("ResourceIdQuantityInput");
            descriptor.Field(c => c.Quantity);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}
