using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryBatch.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetDeliveryBatchAsReadyInputType : SheaftInputType<SetDeliveryBatchAsReadyCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetDeliveryBatchAsReadyCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SetDeliveryBatchAsReadyInput");

            descriptor
                .Field(c => c.Id)
                .ID(nameof(DeliveryBatch))
                .Name("id");
        }
    }
}