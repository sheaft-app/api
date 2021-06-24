using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryBatch.Commands;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateDeliveryBatchInputType : SheaftInputType<UpdateDeliveryBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateDeliveryBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateDeliveryBatchInput");

            descriptor
                .Field(c => c.Id)
                .ID(nameof(DeliveryBatch))
                .Name("id");

            descriptor
                .Field(c => c.Name)
                .Name("name");

            descriptor
                .Field(c => c.Deliveries)
                .Name("deliveries")
                .Type<NonNullType<ListType<ClientDeliveryPositionDtoInputType>>>();
        }
    }
    public class SetNextDeliveryInputType : SheaftInputType<SetNextDeliveryCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetNextDeliveryCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SetNextDeliveryInput");

            descriptor
                .Field(c => c.DeliveryBatchId)
                .ID(nameof(DeliveryBatch))
                .Name("deliveryBatchId");
            
            descriptor
                .Field(c => c.DeliveryId)
                .ID(nameof(Delivery))
                .Name("deliveryId");

            descriptor
                .Field(c => c.StartDelivery)
                .Name("autostartDelivery");
        }
    }
}