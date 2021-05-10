using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.DeliveryModes;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryHoursType : SheaftOutputType<DeliveryHours>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryHours> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<DeliveryHoursByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Day)
                .Name("day");
                
            descriptor
                .Field(c => c.From)
                .Name("from");
                
            descriptor
                .Field(c => c.To)
                .Name("to");
        }
    }
}
