using HotChocolate.Types.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class OrderSortType : SortInputType<OrderDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<OrderDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Status);
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
}
