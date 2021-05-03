using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Business;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BusinessClosingType : SheaftOutputType<BusinessClosing>
    {
        protected override void Configure(IObjectTypeDescriptor<BusinessClosing> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<BusinessClosingsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.ClosedFrom)
                .Name("from");

            descriptor
                .Field(c => c.ClosedTo)
                .Name("to");

            descriptor
                .Field(c => c.Reason)
                .Name("reason");
        }
    }
}