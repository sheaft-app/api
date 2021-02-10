using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateDeliveryModeInputType : SheaftInputType<CreateDeliveryModeInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateDeliveryModeInput> descriptor)
        {
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.MaxPurchaseOrdersPerTimeSlot);
            descriptor.Field(c => c.AutoAcceptRelatedPurchaseOrder);
            descriptor.Field(c => c.AutoCompleteRelatedPurchaseOrder);
            descriptor.Field(c => c.LockOrderHoursBeforeDelivery);

            descriptor.Field(c => c.Address)
                .Type<LocationAddressInputType>();

            descriptor.Field(c => c.OpeningHours)
                .Type<NonNullType<ListType<TimeSlotGroupInputType>>>();
        }
    }
}
