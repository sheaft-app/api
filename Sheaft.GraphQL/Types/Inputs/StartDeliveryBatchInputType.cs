using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryBatch.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class StartDeliveryBatchInputType : SheaftInputType<StartDeliveryBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<StartDeliveryBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("StartDeliveryBatchInput");

            descriptor
                .Field(c => c.DeliveryBatchId)
                .ID(nameof(DeliveryBatch))
                .Name("id");

            descriptor
                .Field(c => c.StartFirstDelivery)
                .Name("startFirstDelivery");
        }
    }
}