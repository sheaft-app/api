using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class DeliveryModeFilterType : FilterInputType<DeliveryModeDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DeliveryModeDto> descriptor)
        {
            descriptor.Name("DeliveryModeFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Kind).AllowIn();
            descriptor.Filter(c => c.Kind).AllowNotIn();
        }
    }
}
