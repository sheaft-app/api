using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.BatchDefinition.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateBatchDefinitionInputType : SheaftInputType<UpdateBatchDefinitionCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateBatchDefinitionCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateBatchDefinitionInput");
            
            descriptor
                .Field(c => c.BatchDefinitionId)
                .ID(nameof(BatchDefinition))
                .Name("id");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.Description)
                .Name("description");

            descriptor
                .Field(c => c.IsDefault)
                .Name("isDefault");
            
            descriptor
                .Field(c => c.FieldDefinitions)
                .Name("fieldDefinitions");
        }
    }
}