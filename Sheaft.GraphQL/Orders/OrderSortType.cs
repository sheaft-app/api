using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Orders
{
    public class OrderSortType : SortInputType<OrderDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<OrderDto> descriptor)
        {
            descriptor.Name("OrderSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.CreatedOn);
        }
    }
}
