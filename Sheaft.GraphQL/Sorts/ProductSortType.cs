using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class ProductSortType : SortInputType<SearchProductDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<SearchProductDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
            descriptor.Sortable(c => c.OnSalePrice);
        }
    }
}
