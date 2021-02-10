using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
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
