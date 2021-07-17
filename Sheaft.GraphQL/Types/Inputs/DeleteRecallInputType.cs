using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Recall.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteRecallInputType : SheaftInputType<DeleteRecallCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteRecallCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteRecallInput");
            
            descriptor
                .Field(c => c.RecallId)
                .ID(nameof(Recall))
                .Name("id");
        }
    }
}