using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class OrderFilterType : FilterInputType<OrderDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<OrderDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Status).AllowIn();
        }
    }
}
