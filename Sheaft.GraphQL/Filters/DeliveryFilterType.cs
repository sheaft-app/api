using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class DeliveryFilterType : FilterInputType<DeliveryDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<DeliveryDto> descriptor)
        {
            descriptor.Name("DeliveryFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Kind);
        }
    }
}
