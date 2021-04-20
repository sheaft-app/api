using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ProductPriceInputType : SheaftInputType<ProductPriceInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProductPriceInputDto> descriptor)
        {
            descriptor.Name("ProductPriceInput");
            descriptor.Field(c => c.ProductId).Name("id").Type<NonNullType<IdType>>();
            descriptor.Field(c => c.WholeSalePricePerUnit);
        }
    }
}