using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryBatch.Commands;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CompleteDeliveryBatchInputType : SheaftInputType<CompleteDeliveryBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CompleteDeliveryBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CompleteDeliveryBatchInput");

            descriptor
                .Field(c => c.DeliveryBatchId)
                .ID(nameof(DeliveryBatch))
                .Name("id");

            descriptor
                .Field(c => c.ReschedulePendingDeliveriesOn)
                .Name("scheduledOn");

            descriptor
                .Field(c => c.From)
                .Name("from");
        }
    }
}