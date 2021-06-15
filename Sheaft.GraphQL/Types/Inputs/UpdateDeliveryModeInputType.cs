using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateDeliveryModeInputType : SheaftInputType<UpdateDeliveryModeCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateDeliveryModeCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateDeliveryModeInput");
            
            descriptor
                .Field(c => c.Description)
                .Name("description");
                
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.Available)
                .Name("available");
                
            descriptor
                .Field(c => c.MaxPurchaseOrdersPerTimeSlot)
                .Name("maxPurchaseOrdersPerTimeSlot");
                
            descriptor
                .Field(c => c.AutoAcceptRelatedPurchaseOrder)
                .Name("autoAcceptRelatedPurchaseOrder");
                
            descriptor
                .Field(c => c.AutoCompleteRelatedPurchaseOrder)
                .Name("autoCompleteRelatedPurchaseOrder");
                
            descriptor
                .Field(c => c.LockOrderHoursBeforeDelivery)
                .Name("lockOrderHoursBeforeDelivery");

            descriptor.Field(c => c.DeliveryModeId)
                .Name("id")
                .ID(nameof(DeliveryMode));

            descriptor.Field(c => c.Address)
                .Name("address")
                .Type<AddressInputType>();

            descriptor.Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.DeliveryHours)
                .Name("deliveryHours")
                .Type<NonNullType<ListType<TimeSlotGroupInputType>>>();

            descriptor.Field(c => c.Closings)
                .Name("closings")
                .Type<ListType<DeliveryClosingInputType>>();

            descriptor.Field(c => c.Agreements)
                .Name("agreements")
                .Type<ListType<AgreementPositionInputType>>();
        }
    }
}