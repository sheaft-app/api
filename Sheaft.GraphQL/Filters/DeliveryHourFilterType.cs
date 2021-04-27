using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class DeliveryHourFilterType : FilterInputType<DeliveryHourDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DeliveryHourDto> descriptor)
        {
            descriptor.Name("DeliveryHourFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
            descriptor.Field(c => c.ExpectedDeliveryDate);
        }
    }
}
