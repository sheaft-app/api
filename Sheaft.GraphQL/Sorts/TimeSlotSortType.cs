using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class TimeSlotSortType : SortInputType<TimeSlotDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<TimeSlotDto> descriptor)
        {
            descriptor.Name("TimeSlotSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Day);
        }
    }
}
