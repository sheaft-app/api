using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.DeliveryBatch.Commands;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class PostponeDeliveryBatchInputType : SheaftInputType<PostponeDeliveryBatchCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PostponeDeliveryBatchCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("PostponeDeliveryBatchInput");

            descriptor
                .Field(c => c.Id)
                .ID(nameof(DeliveryBatch))
                .Name("id");
            
            descriptor
                .Field(c => c.From)
                .Name("from");
            
            descriptor
                .Field(c => c.Reason)
                .Name("reason");
            
            descriptor
                .Field(c => c.ScheduledOn)
                .Name("scheduledOn");
        }
    }
}