using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryBatch.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteDeliveryBatchInputType : SheaftInputType<DeleteDeliveryBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteDeliveryBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteDeliveryBatchInput");

            descriptor
                .Field(c => c.DeliveryBatchId)
                .ID(nameof(DeliveryBatch))
                .Name("id");
        }
    }
}