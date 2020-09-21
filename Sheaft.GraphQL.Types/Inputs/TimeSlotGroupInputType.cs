using HotChocolate.Types;
using Sheaft.GraphQL.Enums;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class TimeSlotGroupInputType : SheaftInputType<TimeSlotGroupInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<TimeSlotGroupInput> descriptor)
        {
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);

            descriptor.Field(c => c.Days)
                .Type<NonNullType<ListType<DayOfWeekEnumType>>>();
        }
    }
}
