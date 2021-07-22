using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Recall.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueSendRecallsInputType : SheaftInputType<QueueSendRecallsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueSendRecallsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("QueueSendRecallsInput");
            
            descriptor
                .Field(c => c.RecallIds)
                .ID(nameof(Recall))
                .Name("ids");
        }
    }
}