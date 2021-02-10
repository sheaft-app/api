using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class ProductQuantityInputType : SheaftInputType<ProductQuantityInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProductQuantityInput> descriptor)
        {
            descriptor.Field(c => c.Quantity);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}
