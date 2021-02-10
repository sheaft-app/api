using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateIdProductsQuantitiesInputType : SheaftInputType<UpdateIdProductsQuantitiesInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateIdProductsQuantitiesInput> descriptor)
        {
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ProductQuantityInputType>>>();
        }
    }
}
