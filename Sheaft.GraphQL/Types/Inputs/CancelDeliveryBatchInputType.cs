using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryBatch.Commands;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CancelDeliveryBatchInputType : SheaftInputType<CancelDeliveryBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CancelDeliveryBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CancelDeliveryBatchInput");

            descriptor
                .Field(c => c.Id)
                .ID(nameof(DeliveryBatch))
                .Name("id");
            
            descriptor
                .Field(c => c.Reason)
                .Name("reason");
        }
    }
}