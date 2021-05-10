using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Products
{
    public class ProductSortType : SortInputType<ProductDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.Name("ProductSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.OnSalePrice);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.WholeSalePrice);

        }
    }
}
