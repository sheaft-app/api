using HotChocolate.Types;
using System;

namespace Sheaft.GraphQL.Enums
{
    public class DayOfWeekEnumType : EnumType<DayOfWeek>
    {
        protected override void Configure(IEnumTypeDescriptor<DayOfWeek> descriptor)
        {
            descriptor.Value(DayOfWeek.Monday).Name("MONDAY");
            descriptor.Value(DayOfWeek.Tuesday).Name("TUESDAY");
            descriptor.Value(DayOfWeek.Wednesday).Name("WEDNESDAY");
            descriptor.Value(DayOfWeek.Thursday).Name("THURSDAY");
            descriptor.Value(DayOfWeek.Friday).Name("FRIDAY");
            descriptor.Value(DayOfWeek.Saturday).Name("SATURDAY");
            descriptor.Value(DayOfWeek.Sunday).Name("SUNDAY");
        }
    }
}
