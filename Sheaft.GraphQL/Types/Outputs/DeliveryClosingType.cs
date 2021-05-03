using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.DeliveryModes;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryClosingType : SheaftOutputType<DeliveryClosing>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryClosing> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<DeliveryClosingsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

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