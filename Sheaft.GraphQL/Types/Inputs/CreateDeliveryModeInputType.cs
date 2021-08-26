using HotChocolate.Types;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateDeliveryModeInputType : SheaftInputType<CreateDeliveryModeCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateDeliveryModeCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateDeliveryModeInput");
            
            descriptor
                .Field(c => c.Description)
                .Name("description");
                
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.Name)
                .Name("name");
                
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
            
            descriptor
                .Field(c => c.DeliveryFeesWholeSalePrice)
                .Name("deliveryFeesWholeSalePrice");
            
            descriptor
                .Field(c => c.DeliveryFeesMinPurchaseOrdersAmount)
                .Name("deliveryFeesMinPurchaseOrdersAmount");
            
            descriptor
                .Field(c => c.ApplyDeliveryFeesWhen)
                .Name("applyDeliveryFeesWhen");
            
            descriptor
                .Field(c => c.AcceptPurchaseOrdersWithAmountGreaterThan)
                .Name("acceptPurchaseOrdersWithAmountGreaterThan");

            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<AddressInputType>();

            descriptor
                .Field(c => c.DeliveryHours)
                .Name("deliveryHours")
                .Type<NonNullType<ListType<TimeSlotGroupInputType>>>();

            descriptor
                .Field(c => c.Closings)
                .Name("closings")
                .Type<ListType<DeliveryClosingInputType>>();
        }
    }
}
