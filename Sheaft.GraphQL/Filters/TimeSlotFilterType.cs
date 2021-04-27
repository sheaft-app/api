using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class TimeSlotFilterType : FilterInputType<TimeSlotDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<TimeSlotDto> descriptor)
        {
            descriptor.Name("TimeSlotFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
}
