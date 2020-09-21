using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class TimeSlotType : ObjectType<TimeSlotDto>
    {
        protected override void Configure(IObjectTypeDescriptor<TimeSlotDto> descriptor)
        {
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
}
