using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Batches;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BatchDefinitionType : SheaftOutputType<BatchDefinition>
    {
        protected override void Configure(IObjectTypeDescriptor<BatchDefinition> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => 
                    ctx.DataLoader<BatchDefinitionsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.Description)
                .Name("description");
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
            
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");
            
            descriptor
                .Field(c => c.FieldDefinitions)
                .Type<ListType<BatchFieldType>>()
                .Name("fieldDefinitions");
        }
    }
}