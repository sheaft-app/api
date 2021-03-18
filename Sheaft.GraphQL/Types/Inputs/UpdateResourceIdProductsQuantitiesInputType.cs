using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateResourceIdProductsQuantitiesInputType : SheaftInputType<UpdateResourceIdProductsQuantitiesDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateResourceIdProductsQuantitiesDto> descriptor)
        {
            descriptor.Name("UpdateResourceIdProductsQuantitiesInput");
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ResourceIdQuantityInputType>>>();
        }
    }
}
