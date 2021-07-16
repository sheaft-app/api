using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.BatchDefinition.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteBatchDefinitionInputType : SheaftInputType<DeleteBatchDefinitionCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteBatchDefinitionCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteBatchDefinitionInput");
            
            descriptor
                .Field(c => c.BatchDefinitionId)
                .ID(nameof(BatchDefinition))
                .Name("id");
        }
    }
}