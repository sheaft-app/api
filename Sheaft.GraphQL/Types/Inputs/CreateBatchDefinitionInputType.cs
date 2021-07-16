using HotChocolate.Types;
using Sheaft.Mediatr.BatchDefinition.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateBatchDefinitionInputType : SheaftInputType<CreateBatchDefinitionCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateBatchDefinitionCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateBatchDefinitionInput");
            
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