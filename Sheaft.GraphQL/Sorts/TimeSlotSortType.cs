using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class TimeSlotSortType : SortInputType<TimeSlotDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<TimeSlotDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.Day);
        }
    }
}
