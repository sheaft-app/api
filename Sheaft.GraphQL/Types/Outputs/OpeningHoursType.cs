using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.GraphQL.Stores;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class OpeningHoursType : SheaftOutputType<Domain.OpeningHours>
    {
        protected override void Configure(IObjectTypeDescriptor<Domain.OpeningHours> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<OpeningHoursByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
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