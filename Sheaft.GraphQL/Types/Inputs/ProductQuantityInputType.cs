using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
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
