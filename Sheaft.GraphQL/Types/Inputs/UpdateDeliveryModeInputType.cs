using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateDeliveryModeInputType : SheaftInputType<UpdateDeliveryModeCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateDeliveryModeCommand> descriptor)
        {
            descriptor.Name("UpdateDeliveryModeInput");
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.MaxPurchaseOrdersPerTimeSlot);
            descriptor.Field(c => c.AutoAcceptRelatedPurchaseOrder);
            descriptor.Field(c => c.AutoCompleteRelatedPurchaseOrder);
            descriptor.Field(c => c.LockOrderHoursBeforeDelivery);

            descriptor.Field(c => c.DeliveryModeId)
                .Name("Id")
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Address)
                .Type<AddressInputType>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.OpeningHours)
                .Type<NonNullType<ListType<TimeSlotGroupInputType>>>();

            descriptor.Field(c => c.Closings)
                .Type<ListType<UpdateOrCreateClosingInputType>>();
        }
    }
}
