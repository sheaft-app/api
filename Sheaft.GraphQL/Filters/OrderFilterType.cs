using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class OrderFilterType : FilterInputType<OrderDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<OrderDto> descriptor)
        {
            descriptor.Name("OrderFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.PurchaseOrdersCount);
        }
    }
}
