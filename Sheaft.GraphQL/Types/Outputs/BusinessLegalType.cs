using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Business;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BusinessLegalType : SheaftOutputType<BusinessLegal>
    {
        protected override void Configure(IObjectTypeDescriptor<BusinessLegal> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<BusinessLegalsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.Validation)
                .Name("validation");
                
            descriptor
                .Field(c => c.Email)
                .Name("email");
                
            descriptor
                .Field(c => c.Address)
                .Name("address");
                
            descriptor
                .Field(c => c.Owner)
                .Name("owner");
        }
    }
}
