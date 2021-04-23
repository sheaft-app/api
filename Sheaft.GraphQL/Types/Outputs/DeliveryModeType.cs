using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Filters;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryModeType : SheaftOutputType<DeliveryModeDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryModeDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.LockOrderHoursBeforeDelivery);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.MaxPurchaseOrdersPerTimeSlot);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.AutoAcceptRelatedPurchaseOrder);
            descriptor.Field(c => c.AutoCompleteRelatedPurchaseOrder);
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Address)
                .Type<AddressType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<UserType>>();

            descriptor.Field(c => c.OpeningHours)
                .Type<ListType<TimeSlotType>>();
            
            descriptor.Field(c => c.Closings)
                .Type<ListType<ClosingType>>();
        }
    }
}
