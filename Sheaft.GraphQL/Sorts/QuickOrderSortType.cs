using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class QuickOrderSortType : SortInputType<QuickOrderDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<QuickOrderDto> descriptor)
        {
            descriptor.Name("QuickOrderSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
        }
    }
}
