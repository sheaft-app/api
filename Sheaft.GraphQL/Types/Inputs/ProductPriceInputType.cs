using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ProductPriceInputType : SheaftInputType<ProductPriceInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ProductPriceInputDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ProductPriceInput");
            
            descriptor
                .Field(c => c.ProductId)
                .Name("id")
                .ID(nameof(Product));
            
            descriptor
                .Field(c => c.WholeSalePricePerUnit)
                .Name("wholeSalePricePerUnit");
        }
    }
}