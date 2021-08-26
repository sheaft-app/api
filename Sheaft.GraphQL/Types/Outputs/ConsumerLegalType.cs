using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Consumers;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ConsumerLegalType : SheaftOutputType<ConsumerLegal>
    {
        protected override void Configure(IObjectTypeDescriptor<ConsumerLegal> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<ConsumerLegalsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.Validation)
                .Name("validation");
                
            descriptor
                .Field(c => c.Owner)
                .Name("owner");
        }
    }
}
