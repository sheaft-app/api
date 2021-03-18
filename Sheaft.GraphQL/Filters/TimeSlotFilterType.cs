using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class TimeSlotFilterType : FilterInputType<TimeSlotDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<TimeSlotDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Day).AllowIn();

            descriptor.Filter(c => c.From).AllowGreaterThan();
            descriptor.Filter(c => c.From).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.From).AllowLowerThan();
            descriptor.Filter(c => c.From).AllowLowerThanOrEquals();

            descriptor.Filter(c => c.To).AllowGreaterThan();
            descriptor.Filter(c => c.To).AllowGreaterThanOrEquals();
            descriptor.Filter(c => c.To).AllowLowerThan();
            descriptor.Filter(c => c.To).AllowLowerThanOrEquals();
        }
    }
}
