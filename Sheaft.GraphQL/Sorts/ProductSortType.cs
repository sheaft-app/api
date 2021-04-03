using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class ProductSortType : SortInputType<ProductDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<ProductDto> descriptor)
        {
            descriptor.Name("ProductSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
            descriptor.Sortable(c => c.OnSalePrice);
        }
    }
}