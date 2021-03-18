using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class TimeSlotGroupInputType : SheaftInputType<TimeSlotGroupDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<TimeSlotGroupDto> descriptor)
        {
            descriptor.Name("TimeSlotGroupInput");
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);

            descriptor.Field(c => c.Days)
                .Type<NonNullType<ListType<DayOfWeekEnumType>>>();
        }
    }
}
