using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Stores
{
    public class SearchStoreSortType : SortInputType<StoreDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<StoreDto> descriptor)
        {
            descriptor.Name("SearchStoreSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Name);
        }
    }
}
