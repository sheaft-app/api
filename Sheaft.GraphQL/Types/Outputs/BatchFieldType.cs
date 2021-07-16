using HotChocolate.Types;
using Sheaft.Domain.Common;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BatchFieldType : SheaftOutputType<BatchField>
    {
        protected override void Configure(IObjectTypeDescriptor<BatchField> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Identifier)
                .Name("identifier");
            
            descriptor
                .Field(c => c.Value)
                .Name("value");
            
            descriptor
                .Field(c => c.Type)
                .Name("type");
            
            descriptor
                .Field(c => c.Required)
                .Name("required");
        }
    }
}