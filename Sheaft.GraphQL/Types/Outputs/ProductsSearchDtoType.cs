using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProductsSearchDtoType : SheaftOutputType<ProductsSearchDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductsSearchDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ProductsSearch");
            
            descriptor
                .Field(c => c.Count)
                .Name("count");
            
            descriptor
                .Field(c => c.Products)
                .Name("products")
                .Type<ListType<ProductType>>();
        }
    }
}
