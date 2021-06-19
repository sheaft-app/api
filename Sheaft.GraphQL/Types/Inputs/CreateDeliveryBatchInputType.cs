using HotChocolate.Types;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateDeliveryBatchInputType : SheaftInputType<CreateDeliveryBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateDeliveryBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateDeliveryBatchInput");

            descriptor
                .Field(c => c.From)
                .Name("from");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.ScheduledOn)
                .Name("scheduledOn");

            descriptor
                .Field(c => c.Deliveries)
                .Name("deliveries")
                .Type<NonNullType<ListType<PurchaseOrderDeliveryPositionDtoInputType>>>();
        }
    }
}