using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class TimeSlotGroupInputType : SheaftInputType<TimeSlotGroupDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<TimeSlotGroupDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("TimeSlotGroupInput");
            
            descriptor
                .Field(c => c.From)
                .Name("from");
            
            descriptor
                .Field(c => c.To)
                .Name("to");

            descriptor
                .Field(c => c.Days)
                .Name("days");
        }
    }
}
