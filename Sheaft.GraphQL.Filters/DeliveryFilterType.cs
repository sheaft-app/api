using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class DeliveryFilterType : FilterInputType<DeliveryDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DeliveryDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Kind).AllowIn();
        }
    }
}
