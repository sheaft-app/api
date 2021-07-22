using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Recall.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteRecallsInputType : SheaftInputType<DeleteRecallsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteRecallsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteRecallsInput");
            
            descriptor
                .Field(c => c.RecallIds)
                .ID(nameof(Recall))
                .Name("ids");
        }
    }
}