using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Recall.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueSendRecallInputType : SheaftInputType<QueueSendRecallCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueSendRecallCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("QueueSendRecallInput");
            
            descriptor
                .Field(c => c.RecallId)
                .ID(nameof(Recall))
                .Name("id");
        }
    }
}