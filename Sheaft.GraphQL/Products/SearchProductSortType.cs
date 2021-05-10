using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Products
{
    public class SearchProductSortType : SortInputType<SearchProductDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<SearchProductDto> descriptor)
        {
            descriptor.Name("SearchProductSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.OnSalePrice);
        }
    }
}
