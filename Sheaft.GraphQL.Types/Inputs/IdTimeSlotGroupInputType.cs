using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class IdTimeSlotGroupInputType : SheaftInputType<IdTimeSlotGroupInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<IdTimeSlotGroupInput> descriptor)
        {
            descriptor.Field(c => c.SelectedHours);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}
