using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class QuickOrderSortType : SortInputType<QuickOrderDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<QuickOrderDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Name);
        }
    }
}
