using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
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
