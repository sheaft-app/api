using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class CreateDeliveryModeInputType : SheaftInputType<CreateDeliveryModeInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateDeliveryModeInput> descriptor)
        {
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.LockOrderHoursBeforeDelivery);

            descriptor.Field(c => c.Address)
                .Type<LocationAddressInputType>();

            descriptor.Field(c => c.OpeningHours)
                .Type<NonNullType<ListType<TimeSlotGroupInputType>>>();
        }
    }
}
