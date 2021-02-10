using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Inputs
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
