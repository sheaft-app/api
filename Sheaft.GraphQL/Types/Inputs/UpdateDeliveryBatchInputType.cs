using HotChocolate.Types;
using Sheaft.Domain;
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
                .Type<NonNullType<ListType<PurchaseOrderDeliveryPositionDtoInputType>>>();
        }
    }
}