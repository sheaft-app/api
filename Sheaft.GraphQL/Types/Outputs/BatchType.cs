using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Batches;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BatchType : SheaftOutputType<Batch>
    {
        protected override void Configure(IObjectTypeDescriptor<Batch> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => 
                    ctx.DataLoader<BatchesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.Number)
                .Name("number");
            
            descriptor
                .Field(c => c.DLC)
                .Name("dlc");
            
            descriptor
                .Field(c => c.DLUO)
                .Name("dluo");
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
            
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
        }
    }
}