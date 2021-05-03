using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.DeliveryModes
{
    public class DeliveryModeFilterType : FilterInputType<DeliveryModeDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DeliveryModeDto> descriptor)
        {
            descriptor.Name("DeliveryModeFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Kind);
        }
    }
}
