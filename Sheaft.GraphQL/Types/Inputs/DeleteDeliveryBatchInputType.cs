using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteDeliveryBatchInputType : SheaftInputType<DeleteDeliveryBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteDeliveryBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteDeliveryBatchInput");

            descriptor
                .Field(c => c.Id)
                .ID(nameof(DeliveryBatch))
                .Name("id");
        }
    }
}