using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryBatch.Commands;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class StartDeliveryBatchInputType : SheaftInputType<StartDeliveryBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<StartDeliveryBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("StartDeliveryBatchInput");

            descriptor
                .Field(c => c.Id)
                .ID(nameof(DeliveryBatch))
                .Name("id");
        }
    }
}