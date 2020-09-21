using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class UpdateDeliveryModeInputType : SheaftInputType<UpdateDeliveryModeInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateDeliveryModeInput> descriptor)
        {
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.LockOrderHoursBeforeDelivery);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Address)
                .Type<LocationAddressInputType>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.OpeningHours)
                .Type<NonNullType<ListType<TimeSlotGroupInputType>>>();
        }
    }
}
